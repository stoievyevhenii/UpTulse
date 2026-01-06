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

        public async Task<MonitoringHistoryResponse> AddNewRecord(MonitoringCreationHistoryRequest request)
        {
            var newHistoryRecord = new MonitoringHistory();
            newHistoryRecord.ApplyFacet(request);

            var createdRecord = await _monitoringHistoryRepository.AddAsync(newHistoryRecord);
            return new MonitoringHistoryResponse(createdRecord);
        }

        public async Task<IEnumerable<MonitoringHistoryResponse>> DeleteOlderThan(DateTimeOffset dateTime)
        {
            var response = await _monitoringHistoryRepository.DeleteRangeAsync(r => r.StartTimeStamp < dateTime);

            if (response is null || response.Count < 1)
            {
                return [];
            }

            return response.Select(r => new MonitoringHistoryResponse(r));
        }

        public async Task<IEnumerable<MonitoringHistoryResponse>> GetAllByTargetAsync(MonitoringTargetHistoryRequest request)
        {
            var records = await _monitoringHistoryRepository.GetAllAsync(r => r.MonitoringTargetId == request.MonitoringTargetId);

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