using UpTulse.Application.Models;
using UpTulse.Core.Entities;

namespace UpTulse.Application.Services
{
    public interface IMonitoringTargetService
    {
        Task<MonitoringTargetResponse> CreateAsync(MonitoringTargetRequest request);

        Task<MonitoringTargetResponse> DeleteAsync(Guid id);

        Task<IEnumerable<MonitoringTargetResponse>> GetAllAsync();

        Task<MonitoringTargetResponse?> GetByIdAsync(Guid id);

        Task<MonitoringTargetResponse> UpdateAsync(Guid id, MonitoringTargetUpdateRequest request);
    }
}