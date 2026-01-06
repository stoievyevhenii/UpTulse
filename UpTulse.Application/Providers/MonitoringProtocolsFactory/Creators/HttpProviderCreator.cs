using UpTulse.Application.Providers.MonitoringProtocolsFactory.Impl;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory.Creators
{
    public class HttpProviderCreator : MonitoringProtocolsProviderCreator
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpProviderCreator(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override IMonitoringProtocolsProvider CreateMonitoringProtocolProvider()
        {
            return new HttpProvider(_httpClientFactory);
        }
    }
}