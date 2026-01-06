using UpTulse.Application.Models;

namespace UpTulse.Application.Services
{
    public interface IMonitoringHistoryService
    {
        Task<MonitoringHistoryResponse> AddNewRecord(MonitoringCreationHistoryRequest request);

        Task<IEnumerable<MonitoringHistoryResponse>> DeleteOlderThan(DateTimeOffset dateTime);

        Task<IEnumerable<MonitoringHistoryResponse>> GetAllByTargetAsync(MonitoringTargetHistoryRequest request);

        Task<MonitoringHistoryResponse> GetAsync(Guid id);
    }
}