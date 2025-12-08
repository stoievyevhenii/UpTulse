using Facet.Extensions;

using UpTulse.Application.Models;
using UpTulse.Core.Entities;
using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.Repositories;

namespace UpTulse.Application.Services.Impl
{
    public class MonitoringTargetService : IMonitoringTargetService
    {
        private readonly IMonitoringTargetRepository _monitoringTargetRepository;

        public MonitoringTargetService(IMonitoringTargetRepository monitoringTargetRepository)
        {
            _monitoringTargetRepository = monitoringTargetRepository;
        }

        public async Task<MonitoringTargetResponse> CreateAsync(MonitoringTargetRequest request)
        {
            var newRecord = new MonitoringTarget
            {
                Name = request.Name,
                Url = request.Url,
                Description = request.Description,
                Method = request.Method,
                //Group = request.Group,
            };

            var response = await _monitoringTargetRepository.AddAsync(newRecord);
            return new MonitoringTargetResponse(response);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _monitoringTargetRepository.DeleteAsync(r => r.Id == id);
            return response != null;
        }

        public async Task<IEnumerable<MonitoringTargetResponse>> GetAllAsync()
        {
            var records = await _monitoringTargetRepository.GetAllAsync();

            var facetsRecords = records.SelectFacets<MonitoringTarget, MonitoringTargetResponse>();

            return facetsRecords;
        }

        public async Task<MonitoringTargetResponse> GetByIdAsync(Guid id)
        {
            var record = await _monitoringTargetRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            return new MonitoringTargetResponse(record);
        }

        public async Task<MonitoringTargetResponse> UpdateAsync(Guid id, MonitoringTargetUpdateRequest request)
        {
            var oldRecord = await _monitoringTargetRepository.GetFirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DbRecordNotFoundException($"Monitoring target record with ID {id} not found");

            oldRecord.Name = string.IsNullOrWhiteSpace(request.Name) ? oldRecord.Name : request.Name;
            oldRecord.Url = string.IsNullOrWhiteSpace(request.Url) ? oldRecord.Url : request.Url;
            oldRecord.Description = string.IsNullOrWhiteSpace(request.Description) ? oldRecord.Description : request.Description;
            oldRecord.Method = request.Method != null ? request.Method.Value : oldRecord.Method;

            var updatedRecord = await _monitoringTargetRepository.UpdateAsync(oldRecord);
            return new MonitoringTargetResponse(updatedRecord);
        }
    }
}