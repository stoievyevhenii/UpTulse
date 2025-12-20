using UpTulse.Application.Models;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory
{
    public interface IMonitoringProtocolsProvider
    {
        Task<bool> PerformCheckAsync(MonitoringParameters monitoringParameters);
    }
}