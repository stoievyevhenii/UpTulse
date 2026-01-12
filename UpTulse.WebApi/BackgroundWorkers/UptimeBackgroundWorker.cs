using System.Collections.Concurrent;
using System.Diagnostics;

using UpTulse.Application.Enums;
using UpTulse.Application.Managers;
using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.DataAccess.EnvironmentVariables;

namespace UpTulse.WebApi.BackgroundWorkers
{
    public class UptimeBackgroundWorker : BackgroundService
    {
        private readonly ILogger<UptimeBackgroundWorker> _logger;
        private readonly IMonitoringTargetsManager _monitoringManager;
        private readonly List<MonitoringResult> _monitoringResults;
        private readonly INotificationSseManager _notificationSseManager;
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _runningMonitors;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Guid> _unavailableTargetsGuids;
        private readonly int _utcOffset;

        public UptimeBackgroundWorker(
            IMonitoringTargetsManager monitoringManager,
            ILogger<UptimeBackgroundWorker> logger,
            IServiceProvider serviceProvider,
            INotificationSseManager notificationSseManager)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _notificationSseManager = notificationSseManager;
            _monitoringManager = monitoringManager;
            _monitoringResults = [];
            _unavailableTargetsGuids = [];
            _runningMonitors = new ConcurrentDictionary<string, CancellationTokenSource>();
            _utcOffset = int.TryParse(Environment.GetEnvironmentVariable(SystemEnv.UTC_OFFSET), out var utcOffset) ? utcOffset : 0;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RetrieveAllTargetsForMonitoring();

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Reactive Uptime Worker Started.");
            }

            try
            {
                await foreach (var op in _monitoringManager.OperationReader.ReadAllAsync(stoppingToken))
                {
                    switch (op.Type)
                    {
                        case OperationType.Add:
                            StartMonitor(op.Target);
                            break;

                        case OperationType.Remove:
                            StopMonitor(op.Target.Name);
                            break;
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogCritical(ex, "UptimeWorker critical error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reactive Uptime Worker encountered a fatal error reading operations.");
            }
        }

        private void LogInfo(MonitoringResult result)
        {
            Console.WriteLine($"-- {result.Name}: ({(result.IsUp ? "UP" : "DOWN")}) TIMESTAMP: {DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(_utcOffset))}");
        }

        private async Task MonitoringTargetStateIsChanged(MonitoringResult result, bool isUp)
        {
            try
            {
                using var notificationScope = _serviceProvider.CreateScope();
                var monitoringTargetsService = notificationScope.ServiceProvider.GetRequiredService<IMonitoringTargetService>();

                var monitoringTarget = await monitoringTargetsService.GetByIdAsync(result.TargetId);

                if (monitoringTarget == null)
                {
                    _logger.LogWarning("Monitoring target not found: {TargetId}", result.TargetId);
                    return;
                }

                var notificationChannelProviderService = notificationScope.ServiceProvider
                    .GetRequiredService<INotificationChannelProviderResolver>();

                var notificationProviderService = notificationChannelProviderService
                    .GetProviderCreator(monitoringTarget.NotificationChannel);

                var dateTimeWithOffset = result.EndTimeStamp.ToOffset(TimeSpan.FromHours(_utcOffset));

                await notificationProviderService.SendMessageAsync(new()
                {
                    Subject = isUp ? $"🟢 {monitoringTarget.Name} is UP" : $"🔴 {monitoringTarget.Name} is DOWN",
                    Body = dateTimeWithOffset.ToString("F"),
                    IsUp = isUp,
                    IsAvailabilityCritical = monitoringTarget.IsAvailabilityCritical,
                    IsUnavailabilityCritical = monitoringTarget.IsUnavailabilityCritical
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification for {Name}", result.Name);
            }
        }

        private async Task MonitorLoopAsync(MonitoringTargetRequest target, CancellationToken token)
        {
            using var timer = new PeriodicTimer(target.Interval);

            try
            {
                do
                {
                    var result = await PerformCheckAsync(target, token);
                    await SaveAndBroadcastResult(result);
                }

                while (await timer.WaitForNextTickAsync(token));
            }
            catch (OperationCanceledException)
            {
                _ = 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical failure in monitor {Name}", target.Name);
            }
        }

        private async Task<MonitoringResult> PerformCheckAsync(MonitoringTargetRequest target, CancellationToken token)
        {
            long elapsedMs = 0;
            bool isUp = false;
            var startTime = DateTimeOffset.UtcNow;

            var sw = Stopwatch.StartNew();

            try
            {
                using var protocolResolverScope = _serviceProvider.CreateScope();
                var protocolResolverService = protocolResolverScope.ServiceProvider.GetRequiredService<IMonitoringProtocolResolver>();
                var protocolResolver = protocolResolverService.GetProtocolProvider(target.Protocol);

                isUp = await protocolResolver.PerformCheckAsync(new()
                {
                    Address = target.Address,
                    CancellationToken = token
                });
            }
            catch
            {
                isUp = false;
            }

            sw.Stop();
            elapsedMs = sw.ElapsedMilliseconds;

            return new MonitoringResult(target.Name, isUp, elapsedMs, target.Id, startTime, DateTimeOffset.UtcNow);
        }

        private async Task RetrieveAllTargetsForMonitoring()
        {
            using var scope = _serviceProvider.CreateScope();
            var monitoringTargetService = scope.ServiceProvider.GetRequiredService<IMonitoringTargetService>();
            var monitoringTargetsList = await monitoringTargetService.GetAllAsync();

            foreach (var monitoringTarget in monitoringTargetsList)
            {
                StartMonitor(new()
                {
                    Id = monitoringTarget.Id,
                    Address = monitoringTarget.Address,
                    Description = monitoringTarget.Description,
                    GroupId = monitoringTarget.Group != null ? monitoringTarget.Group.Id : Guid.Empty,
                    Interval = monitoringTarget.Interval,
                    Name = monitoringTarget.Name,
                    Protocol = monitoringTarget.Protocol
                });
            }
        }

        private async Task SaveAndBroadcastResult(MonitoringResult result)
        {
            LogInfo(result);

            using var monitoringScope = _serviceProvider.CreateScope();
            var monitoringHistoryService = monitoringScope.ServiceProvider.GetRequiredService<IMonitoringHistoryService>();

            await monitoringHistoryService.AddNewRecord(new()
            {
                IsUp = result.IsUp,
                MonitoringTargetId = result.TargetId,
                StartTimeStamp = result.StartTimeStamp,
                EndTimeStamp = result.EndTimeStamp,
                ResponseTimeInMs = result.ResponseTimeMs,
            });

            await _notificationSseManager.BroadcastAsync(result);

            bool? isUpStateChange = null;

            lock (_monitoringResults)
            {
                var targetHistory = _monitoringResults
                    .Where(r => r.TargetId == result.TargetId)
                    .OrderBy(r => r.StartTimeStamp)
                    .ToList();

                if (targetHistory.Count < 3)
                {
                    _monitoringResults.Add(result);
                }
                else
                {
                    var allDown = targetHistory.All(r => !r.IsUp);
                    var isCurrentlyUnavailable = _unavailableTargetsGuids.Contains(result.TargetId);

                    if (!allDown && isCurrentlyUnavailable)
                    {
                        _unavailableTargetsGuids.Remove(result.TargetId);
                        isUpStateChange = true;
                    }
                    else if (allDown && !isCurrentlyUnavailable)
                    {
                        _unavailableTargetsGuids.Add(result.TargetId);
                        isUpStateChange = false;
                    }

                    _monitoringResults.RemoveAll(r => r.TargetId == result.TargetId);
                }
            }

            if (isUpStateChange.HasValue)
            {
                await MonitoringTargetStateIsChanged(result, isUpStateChange.Value);
            }
        }

        private void StartMonitor(MonitoringTargetRequest target)
        {
            StopMonitor(target.Name);

            var cts = new CancellationTokenSource();
            if (_runningMonitors.TryAdd(target.Name, cts))
            {
                _ = Task.Run(() => MonitorLoopAsync(target, cts.Token), cts.Token);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Started monitoring {Name} every {Sec}s", target.Name, target.Interval.TotalSeconds);
                }
            }
        }

        private void StopMonitor(string name)
        {
            if (_runningMonitors.TryRemove(name, out var cts))
            {
                cts.Cancel();
                cts.Dispose();

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Stopped monitoring {Name}", name);
                }
            }
        }
    }
}