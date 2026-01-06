using UpTulse.Application.Providers.MonitoringProtocolsFactory;
using UpTulse.Shared.Enums;

namespace UpTulse.Application.Services
{
    public interface IMonitoringProtocolResolver
    {
        MonitoringProtocolsProviderCreator GetProtocolProvider(MonitoringProtocol monitoringProtocol);
    }
}