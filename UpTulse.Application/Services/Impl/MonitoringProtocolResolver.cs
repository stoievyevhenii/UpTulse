using UpTulse.Application.Providers.MonitoringProtocolsFactory;
using UpTulse.Application.Providers.MonitoringProtocolsFactory.Creators;
using UpTulse.Shared.Enums;

namespace UpTulse.Application.Services.Impl
{
    public class MonitoringProtocolResolver : IMonitoringProtocolResolver
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MonitoringProtocolResolver(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public MonitoringProtocolsProviderCreator GetProtocolProvider(MonitoringProtocol monitoringProtocol)
        {
            return monitoringProtocol switch
            {
                MonitoringProtocol.HTTP => new HttpProviderCreator(_httpClientFactory),
                MonitoringProtocol.Ping => new PingProviderCreator(),
                _ => throw new NotImplementedException()
            };
        }
    }
}