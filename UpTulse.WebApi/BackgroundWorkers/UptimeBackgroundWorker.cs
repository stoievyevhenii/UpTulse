using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;

using Microsoft.Extensions.Logging;

using UpTulse.Application.Enums;
using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Shared.Enums;

namespace UpTulse.WebApi.BackgroundWorkers
{
    public class UptimeBackgroundWorker : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UptimeBackgroundWorker> _logger;
        private readonly IMonitoringManagerService _monitoringManager;
        private readonly Dictionary<string, CancellationTokenSource> _runningMonitors;
        private readonly IServiceProvider _serviceProvider;

        public UptimeBackgroundWorker(
            IMonitoringManagerService monitoringManager,
            ILogger<UptimeBackgroundWorker> logger,
            IServiceProvider serviceProvider,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _monitoringManager = monitoringManager;
            _runningMonitors = [];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reactive Uptime Worker Started.");

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

        private void LogResult(MonitoringResult result)
        {
            _logger.LogCritical("[{Time}] {Name}: {Status} ({Ms}ms)",
               result.EndTimeStamp.ToString(), result.Name, result.IsUp ? "UP" : "DOWN", result.ResponseTimeMs);

            using var scope = _serviceProvider.CreateScope();
            var monitoringHistoryService = scope.ServiceProvider.GetRequiredService<IMonitoringHistoryService>();

            //monitoringHistoryService.AddNewRecord(new()
            //{
            //    IsUp = result.IsUp,
            //    MonitoringTargetId = result.TargetId,
            //    StartTimeStamp = result.StartTimeStamp,
            //    EndTimeStamp = result.EndTimeStamp,
            //    ResponseTimeInMs = result.ResponseTimeMs
            //});
        }

        private async Task MonitorLoopAsync(MonitoringTargetRequest target, CancellationToken token)
        {
            using var timer = new PeriodicTimer(target.Interval);

            try
            {
                do
                {
                    var result = await PerformCheckAsync(target, token);
                    LogResult(result);
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
                if (target.Protocol == MonitoringProtocol.HTTP)
                {
                    using var client = _httpClientFactory.CreateClient();
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = await client.GetAsync(target.Address, token);
                    isUp = response.IsSuccessStatusCode;
                }
                else
                {
                    using var ping = new Ping();
                    var reply = await ping.SendPingAsync(hostNameOrAddress: target.Address, timeout: target.Interval, cancellationToken: token);
                    isUp = reply.Status == IPStatus.Success;
                }
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

            _logger.LogInformation("Started monitoring {Name} every {Sec}s", target.Name, target.Interval.TotalSeconds);
        }

        private void StopMonitor(string name)
        {
            if (_runningMonitors.TryGetValue(name, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                _runningMonitors.Remove(name);
                _logger.LogInformation("Stopped monitoring {Name}", name);
            }
        }
    }
}