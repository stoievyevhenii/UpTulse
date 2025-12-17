using System;
using System.Collections.Generic;
using System.Text;

using Quartz;

using UpTulse.Application.Environments;
using UpTulse.Application.Services;

namespace UpTulse.Application.Schedulers
{
    public class HistoryCleaner : IJob
    {
        private readonly int _deleteAfter;
        private readonly IMonitoringHistoryService _monitoringHistoryService;

        public HistoryCleaner(IMonitoringHistoryService monitoringHistoryService)
        {
            _monitoringHistoryService = monitoringHistoryService;
            _deleteAfter = int.TryParse(Environment.GetEnvironmentVariable(SchedulersEnv.HISTORY_CLEANER_SCHEDULE), out var days)
                ? days
                : 7;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var records = await _monitoringHistoryService
                .DeleteOlderThan(DateTime.UtcNow.AddDays(_deleteAfter));

            Console.WriteLine($"HistoryCleaner executed at {DateTime.Now} RECORDS COUNT: {records.Count()}");
        }
    }
}