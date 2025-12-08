using Facet.Extensions;
using Facet.Mapping;

using UpTulse.Application.Models;
using UpTulse.Core.Entities;
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
                Group = request.Group,
            };

            var response = await _monitoringTargetRepository.AddAsync(newRecord);
            return new MonitoringTargetResponse(response);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MonitoringTargetResponse>> GetAllAsync()
        {
            var records = await _monitoringTargetRepository.GetAllAsync();

            var facetsRecords = await records.ToFacetsParallelAsync<MonitoringTargetResponse, MonitoringTarget>();

            return facetsRecords;
        }

        public Task<MonitoringTargetResponse> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MonitoringTargetResponse> UpdateAsync(Guid id, MonitoringTargetRequest request)
        {
            throw new NotImplementedException();
        }
    }
}