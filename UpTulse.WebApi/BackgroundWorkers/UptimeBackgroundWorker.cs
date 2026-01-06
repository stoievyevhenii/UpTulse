using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;

using Microsoft.Extensions.Logging;

using UpTulse.Application.Enums;
using UpTulse.Application.EnvironmentVariables;
using UpTulse.Application.Managers;
using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Shared.Enums;

namespace UpTulse.WebApi.BackgroundWorkers
{
    public class UptimeBackgroundWorker : BackgroundService
    {
        private readonly ILogger<UptimeBackgroundWorker> _logger;
        private readonly IMonitoringTargetsManager _monitoringManager;
        private readonly List<MonitoringResult> _monitoringResults;
        private readonly INotificationSseManager _notificationSseManager;
        private readonly Dictionary<string, CancellationTokenSource> _runningMonitors;
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
            _runningMonitors = [];
            _monitoringResults = [];
            _unavailableTargetsGuids = [];

            _utcOffset = int.TryParse(Environment.GetEnvironmentVariable(SystemEnv.UTC_OFFSET), out var utcOffset) ? utcOffset : 0;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Reactive Uptime Worker Started.");
                    }

                    await RetrieveAllTargetsForMonitoring();

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
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Reactive Uptime Worker error.");
                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task AnalyzeResultsAndLog(MonitoringResult result)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical("[{Time}] {Name}: {Status} ({Ms}ms)",
                   result.EndTimeStamp.ToString(), result.Name, result.IsUp ? "UP" : "DOWN", result.ResponseTimeMs);
            }

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

            var searchebleRecords = new List<MonitoringResult>();

            if (_monitoringResults.Any(r => r.TargetId == result.TargetId))
            {
                searchebleRecords = [.. _monitoringResults
                 .Where(r => r.TargetId == result.TargetId)
                 .OrderBy(r => r.StartTimeStamp)];
            }

            if (searchebleRecords.Count < 3)
            {
                _monitoringResults.Add(result);
            }
            else
            {
                var allDown = searchebleRecords.All(r => !r.IsUp);

                if (!allDown && _unavailableTargetsGuids.Contains(result.TargetId))
                {
                    _unavailableTargetsGuids.Remove(result.TargetId);
                    await MonitoringTargetStateIsChanged(result, true);
                }

                if (allDown && !_unavailableTargetsGuids.Contains(result.TargetId))
                {
                    _unavailableTargetsGuids.Add(result.TargetId);
                    await MonitoringTargetStateIsChanged(result, false);
                }

                _monitoringResults.RemoveAll(r => r.TargetId == result.TargetId);
            }
        }

        private async Task MonitoringTargetStateIsChanged(MonitoringResult result, bool isUp)
        {
            using var notificationScope = _serviceProvider.CreateScope();

            var monitoringTargetsService = notificationScope.ServiceProvider
                .GetRequiredService<IMonitoringTargetService>();

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
            });
        }

        private async Task MonitorLoopAsync(MonitoringTargetRequest target, CancellationToken token)
        {
            using var timer = new PeriodicTimer(target.Interval);

            try
            {
                do
                {
                    var result = await PerformCheckAsync(target, token);
                    await AnalyzeResultsAndLog(result);
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
            var sw = Stopwatch.StartNew();
            bool isUp = false;
            var startTime = DateTimeOffset.UtcNow;

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
            catch { isUp = false; }

            sw.Stop();

            return new MonitoringResult(target.Name, isUp, sw.ElapsedMilliseconds, target.Id, startTime, DateTimeOffset.UtcNow);
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

        private void StartMonitor(MonitoringTargetRequest target)
        {
            StopMonitor(target.Name);

            var cts = new CancellationTokenSource();
            _runningMonitors[target.Name] = cts;

            _ = Task.Run(() => MonitorLoopAsync(target, cts.Token), cts.Token);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Started monitoring {Name} every {Sec}s", target.Name, target.Interval.TotalSeconds);
            }
        }

        private void StopMonitor(string name)
        {
            if (_runningMonitors.TryGetValue(name, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                _runningMonitors.Remove(name);
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Stopped monitoring {Name}", name);
                }
            }
        }
    }
}