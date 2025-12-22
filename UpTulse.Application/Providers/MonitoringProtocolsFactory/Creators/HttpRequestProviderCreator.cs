using UpTulse.Application.Providers.MonitoringProtocolsFactory.Impl;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory.Creators
{
    public class HttpRequestProviderCreator : MonitoringProtocolsProviderCreator
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpRequestProviderCreator(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override IMonitoringProtocolsProvider CreateMonitoringProtocolProvider()
        {
            return new HttpRequestProvider(_httpClientFactory);
        }
    }
}