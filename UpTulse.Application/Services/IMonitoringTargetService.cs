using UpTulse.Application.Models;

namespace UpTulse.Application.Services
{
    public interface IMonitoringTargetService
    {
        Task<MonitoringTargetResponse> CreateAsync(MonitoringTargetRequest request);

        Task DeleteAsync(Guid id);

        Task<List<MonitoringTargetResponse>> GetAllAsync();

        Task<MonitoringTargetResponse> GetByIdAsync(Guid id);

        Task<MonitoringTargetResponse> UpdateAsync(Guid id, MonitoringTargetRequest request);
    }
}