using Microsoft.AspNetCore.Mvc;

using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Shared.Models;

namespace UpTulse.WebApi.Controllers.Api
{
    public class MonitoringHistoriesController : ApiController
    {
        private readonly IMonitoringHistoryService _monitoringHistoryService;

        public MonitoringHistoriesController(IMonitoringHistoryService monitoringHistoryService)
        {
            _monitoringHistoryService = monitoringHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByTargetAsync([FromQuery] MonitoringTargetHistoryRequest request)
        {
            return Ok(ApiResult<IEnumerable<MonitoringHistoryResponse>>
                .Success(await _monitoringHistoryService.GetAllByTargetAsync(request)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(ApiResult<MonitoringHistoryResponse>
                .Success(await _monitoringHistoryService.GetAsync(id)));
        }
    }
}