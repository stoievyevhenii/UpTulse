using UpTulse.Application.Models;

namespace UpTulse.Application.Services
{
    public interface IMonitoringHistoryService
    {
        Task<MonitoringHistoryResponse> AddNewRecord(MonitoringHistoryRequest request);

        Task<IEnumerable<MonitoringHistoryResponse>> DeleteOlderThan(DateTimeOffset dateTime);

        Task<IEnumerable<MonitoringHistoryResponse>> GetAllByTargetAsync(MonitoringHistoryRequest request);

        Task<MonitoringHistoryResponse> GetAsync(Guid id);
    }
}