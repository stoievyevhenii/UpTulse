using Microsoft.AspNetCore.Mvc;

using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Core.Entities;
using UpTulse.Shared.Models;

namespace UpTulse.WebApi.Controllers.Api
{
    public class MonitoringTargetsController : ApiController
    {
        private readonly IMonitoringTargetService _monitoringTargetService;

        public MonitoringTargetsController(IMonitoringTargetService monitoringTargetService)
        {
            _monitoringTargetService = monitoringTargetService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(ApiResult<MonitoringTargetResponse>.Success(await _monitoringTargetService.DeleteAsync(id)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(ApiResult<MonitoringTargetResponse?>.Success(await _monitoringTargetService.GetByIdAsync(id)));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(ApiResult<IEnumerable<MonitoringTargetResponse>>.Success(await _monitoringTargetService.GetAllAsync()));
        }

        [HttpPost]
        public async Task<IActionResult> Post(MonitoringTargetRequest request)
        {
            return Ok(ApiResult<MonitoringTargetResponse>.Success(await _monitoringTargetService.CreateAsync(request)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, MonitoringTargetUpdateRequest request)
        {
            return Ok(ApiResult<MonitoringTargetResponse>.Success(await _monitoringTargetService.UpdateAsync(id, request)));
        }
    }
}