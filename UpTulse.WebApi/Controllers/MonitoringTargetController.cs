using Microsoft.AspNetCore.Mvc;

using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Shared.Models;

namespace UpTulse.WebApi.Controllers
{
    public class MonitoringTargetController : ApiController
    {
        private readonly IMonitoringTargetService _monitoringTargetService;

        public MonitoringTargetController(IMonitoringTargetService monitoringTargetService)
        {
            _monitoringTargetService = monitoringTargetService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(ApiResult<IEnumerable<MonitoringTargetResponse>>.Success(await _monitoringTargetService.GetAllAsync()));
        }
    }
}