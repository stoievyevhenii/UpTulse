using System;
using System.Collections.Generic;
using System.Text;

using Quartz;

using UpTulse.Application.Services;

namespace UpTulse.Application.Schedulers
{
    public class HistoryCleaner : IJob
    {
        private readonly IMonitoringHistoryService _monitoringHistoryService;

        public HistoryCleaner(IMonitoringHistoryService monitoringHistoryService)
        {
            _monitoringHistoryService = monitoringHistoryService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var records = await _monitoringHistoryService
                .DeleteOlderThan(DateTime.UtcNow.AddDays(-7));

            Console.WriteLine($"HistoryCleaner executed at {DateTime.Now} RECORDS COUNT: {records.Count()}");
        }
    }
}