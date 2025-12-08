using UpTulse.Application.Models;

namespace UpTulse.Application.Services
{
    public interface IMonitoringGroupService
    {
        Task<MonitoringGroupResponse> CreateAsync(MonitoringGroupRequest request);

        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<MonitoringGroupResponse>> GetAllAsync();

        Task<MonitoringGroupResponse> GetByIdAsync(Guid id);

        Task<MonitoringGroupResponse> UpdateAsync(Guid id, MonitoringGroupUpdateRequest request);
    }
}