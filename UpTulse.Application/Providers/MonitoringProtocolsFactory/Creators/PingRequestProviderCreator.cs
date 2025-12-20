using UpTulse.Application.Providers.MonitoringProtocolsFactory.Impl;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory.Creators
{
    public class PingRequestProviderCreator : MonitoringProtocolsProviderCreator
    {
        public override IMonitoringProtocolsProvider CreateMonitoringProtocolProvider()
        {
            return new PingRequestProvider();
        }
    }
}