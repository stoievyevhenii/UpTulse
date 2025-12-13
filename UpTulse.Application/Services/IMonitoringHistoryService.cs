using UpTulse.Application.Models;

namespace UpTulse.Application.Services
{
    public interface IMonitoringHistoryService
    {
        Task<IEnumerable<MonitoringHistoryResponse>> DeleteOlderThan(DateTimeOffset dateTime);

        Task<IEnumerable<MonitoringHistoryResponse>> GetAllByTargetAsync(Guid targetId);

        Task<MonitoringHistoryResponse> GetAsync(Guid id);
    }
}