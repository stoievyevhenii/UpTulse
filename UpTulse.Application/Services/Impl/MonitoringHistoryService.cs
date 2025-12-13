using Facet.Extensions;

using UpTulse.Application.Models;
using UpTulse.Core.Entities;
using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.Repositories;

namespace UpTulse.Application.Services.Impl
{
    public class MonitoringHistoryService : IMonitoringHistoryService
    {
        private readonly IMonitoringHistoryRepository _monitoringHistoryRepository;

        public MonitoringHistoryService(IMonitoringHistoryRepository monitoringHistoryRepository)
        {
            _monitoringHistoryRepository = monitoringHistoryRepository;
        }

        public async Task<IEnumerable<MonitoringHistoryResponse>> DeleteOlderThan(DateTimeOffset dateTime)
        {
            var response = await _monitoringHistoryRepository.DeleteRangeAsync(r => r.TimeStamp < dateTime);

            if (response is null || response.Count < 1)
            {
                return [];
            }

            return response.Select(r => new MonitoringHistoryResponse(r));
        }

        public async Task<IEnumerable<MonitoringHistoryResponse>> GetAllByTargetAsync(Guid targetId)
        {
            var records = await _monitoringHistoryRepository.GetAllAsync(r => r.MonitoringTargetId == targetId);

            if (records is null || records.Count < 1)
            {
                return [];
            }

            return records.SelectFacets<MonitoringHistory, MonitoringHistoryResponse>();
        }

        public async Task<MonitoringHistoryResponse> GetAsync(Guid id)
        {
            var record = await _monitoringHistoryRepository.GetFirstOrDefaultAsync(r => r.Id == id);

            return record is null
                ? throw new DbRecordNotFoundException("Monitoring history record not found.")
                : new MonitoringHistoryResponse(record);
        }
    }
}