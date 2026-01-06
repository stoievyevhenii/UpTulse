using Microsoft.AspNetCore.Mvc;

using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Shared.Models;

namespace UpTulse.WebApi.Controllers.Api
{
    public class MonitoringGroupsController : ApiController
    {
        private readonly IMonitoringGroupService _monitoringGroupService;

        public MonitoringGroupsController(IMonitoringGroupService monitoringGroupService)
        {
            _monitoringGroupService = monitoringGroupService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(ApiResult<bool>.Success(await _monitoringGroupService.DeleteAsync(id)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(ApiResult<MonitoringGroupResponse?>.Success(await _monitoringGroupService.GetByIdAsync(id)));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(ApiResult<IEnumerable<MonitoringGroupResponse>>.Success(await _monitoringGroupService.GetAllAsync()));
        }

        [HttpPost]
        public async Task<IActionResult> Post(MonitoringGroupRequest request)
        {
            return Ok(ApiResult<MonitoringGroupResponse>.Success(await _monitoringGroupService.CreateAsync(request)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, MonitoringGroupUpdateRequest request)
        {
            return Ok(ApiResult<MonitoringGroupResponse>.Success(await _monitoringGroupService.UpdateAsync(id, request)));
        }
    }
}