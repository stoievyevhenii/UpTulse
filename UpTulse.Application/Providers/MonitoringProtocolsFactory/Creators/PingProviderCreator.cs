using UpTulse.Application.Providers.MonitoringProtocolsFactory.Impl;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory.Creators
{
    public class PingProviderCreator : MonitoringProtocolsProviderCreator
    {
        public override IMonitoringProtocolsProvider CreateMonitoringProtocolProvider()
        {
            return new PingProvider();
        }
    }
}