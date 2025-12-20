using UpTulse.Application.Models;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory.Impl
{
    public class PingRequestProvider : IMonitoringProtocolsProvider
    {
        public Task<bool> PerformCheckAsync(MonitoringParameters monitoringParameters)
        {
            throw new NotImplementedException();
        }
    }
}