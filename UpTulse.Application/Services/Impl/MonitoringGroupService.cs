using Facet.Extensions;

using UpTulse.Application.Models;
using UpTulse.Core.Entities;
using UpTulse.DataAccess.Repositories;

namespace UpTulse.Application.Services.Impl
{
    public class MonitoringGroupService : IMonitoringGroupService
    {
        private readonly IMonitoringGroupRepository _monitoringGroupRepository;

        public MonitoringGroupService(IMonitoringGroupRepository monitoringGroupRepository)
        {
            _monitoringGroupRepository = monitoringGroupRepository;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            return await _monitoringGroupRepository.AnyAsync(r => r.Id == id);
        }

        public async Task<MonitoringGroupResponse> CreateAsync(MonitoringGroupRequest request)
        {
            var response = await _monitoringGroupRepository.AddAsync(new()
            {
                Name = request.Name,
            });

            return new MonitoringGroupResponse(response);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _monitoringGroupRepository.DeleteAsync(r => r.Id == id);
            return response != null;
        }

        public async Task<IEnumerable<MonitoringGroupResponse>> GetAllAsync()
        {
            var records = await _monitoringGroupRepository.GetAllAsync();

            return records.SelectFacets<MonitoringGroup, MonitoringGroupResponse>();
        }

        public async Task<MonitoringGroupResponse?> GetByIdAsync(Guid id)
        {
            var record = await _monitoringGroupRepository.GetFirstOrDefaultAsync(r => r.Id == id);

            if (record is null)
            {
                return null;
            }

            return new MonitoringGroupResponse(record);
        }

        public async Task<MonitoringGroupResponse> UpdateAsync(Guid id, MonitoringGroupUpdateRequest request)
        {
            var oldRecord = await _monitoringGroupRepository.GetFirstOrDefaultAsync(r => r.Id == id);

            oldRecord.Name = string.IsNullOrWhiteSpace(request.Name) ? oldRecord.Name : request.Name;

            var updatedRecord = await _monitoringGroupRepository.UpdateAsync(oldRecord);
            return new MonitoringGroupResponse(updatedRecord);
        }
    }
}