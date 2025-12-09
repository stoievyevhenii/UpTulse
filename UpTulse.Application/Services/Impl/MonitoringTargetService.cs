using Facet.Extensions;
using Facet.Mapping;

using UpTulse.Application.MapperConfigs;
using UpTulse.Application.Models;
using UpTulse.Core.Entities;
using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.Repositories;

namespace UpTulse.Application.Services.Impl
{
    public class MonitoringTargetService : IMonitoringTargetService
    {
        private readonly MonitoringTargetMapperWithDi _monitoringTargetMapperWithDi;
        private readonly IMonitoringTargetRepository _monitoringTargetRepository;

        public MonitoringTargetService(MonitoringTargetMapperWithDi monitoringTargetMapperWithDi, IMonitoringTargetRepository monitoringTargetRepository)
        {
            _monitoringTargetRepository = monitoringTargetRepository;
            _monitoringTargetMapperWithDi = monitoringTargetMapperWithDi;
        }

        public async Task<MonitoringTargetResponse> CreateAsync(MonitoringTargetRequest request)
        {
            var newMonitoringTarget = new MonitoringTarget();

            newMonitoringTarget.ApplyFacet(request);

            var response = await _monitoringTargetRepository.AddAsync(newMonitoringTarget);

            return await response.ToFacetAsync(_monitoringTargetMapperWithDi);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _monitoringTargetRepository.DeleteAsync(r => r.Id == id);
            return response != null;
        }

        public async Task<IEnumerable<MonitoringTargetResponse>> GetAllAsync()
        {
            var records = await _monitoringTargetRepository.GetAllAsync();

            var mappedList = await records.ToFacetsAsync(_monitoringTargetMapperWithDi);

            return mappedList;
        }

        public async Task<MonitoringTargetResponse> GetByIdAsync(Guid id)
        {
            var record = await _monitoringTargetRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            return await record.ToFacetAsync(_monitoringTargetMapperWithDi);
        }

        public async Task<MonitoringTargetResponse> UpdateAsync(Guid id, MonitoringTargetUpdateRequest request)
        {
            var oldRecord = await _monitoringTargetRepository.GetFirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DbRecordNotFoundException($"Monitoring target record with ID {id} not found");

            oldRecord.Name = string.IsNullOrWhiteSpace(request.Name) ? oldRecord.Name : request.Name;
            oldRecord.Url = string.IsNullOrWhiteSpace(request.Url) ? oldRecord.Url : request.Url;
            oldRecord.Description = string.IsNullOrWhiteSpace(request.Description) ? oldRecord.Description : request.Description;
            oldRecord.Method = request.Method != null ? request.Method.Value : oldRecord.Method;

            if (request.GroupId is Guid)
            {
                oldRecord.GroupId = request.GroupId != null && request.GroupId != Guid.Empty ? request.GroupId.Value : oldRecord.GroupId;
            }

            var updatedRecord = await _monitoringTargetRepository.UpdateAsync(oldRecord);
            return await updatedRecord.ToFacetAsync(_monitoringTargetMapperWithDi);
        }
    }
}