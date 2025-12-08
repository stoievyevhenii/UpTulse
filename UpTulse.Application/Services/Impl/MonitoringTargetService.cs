using UpTulse.Application.Models;
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

        public Task<MonitoringTargetResponse> CreateAsync(MonitoringTargetRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MonitoringTargetResponse>> GetAllAsync()
        {
            throw new NotImplementedException();
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