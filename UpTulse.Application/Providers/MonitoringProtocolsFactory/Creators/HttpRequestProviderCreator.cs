using UpTulse.Application.Providers.MonitoringProtocolsFactory.Impl;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory.Creators
{
    public class HttpRequestProviderCreator : MonitoringProtocolsProviderCreator
    {
        public override IMonitoringProtocolsProvider CreateMonitoringProtocolProvider()
        {
            return new HttpRequestProvider();
        }
    }
}