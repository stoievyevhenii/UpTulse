using Microsoft.AspNetCore.Mvc;

using UpTulse.Application.Models;
using UpTulse.Application.Services;

namespace UpTulse.WebApi.Controllers
{
    public class MonitoringHistoriesController : ApiController
    {
        private readonly IMonitoringHistoryService _monitoringHistoryService;

        public MonitoringHistoriesController(IMonitoringHistoryService monitoringHistoryService)
        {
            _monitoringHistoryService = monitoringHistoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<MonitoringHistoryResponse>> GetAllByTargetAsync(MonitoringHistoryRequest request)
        {
            return await _monitoringHistoryService.GetAllByTargetAsync(request);
        }

        [HttpGet("{id}")]
        public async Task<MonitoringHistoryResponse> GetAsync(Guid id)
        {
            return await _monitoringHistoryService.GetAsync(id);
        }
    }
}