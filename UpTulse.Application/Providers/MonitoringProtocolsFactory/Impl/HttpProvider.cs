using System.Net.Sockets;
using System.Threading;

using Microsoft.Extensions.Hosting;

using UpTulse.Application.Models;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory.Impl
{
    public class HttpProvider : IMonitoringProtocolsProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> PerformCheckAsync(MonitoringParameters monitoringParameters)
        {
            using var client = _httpClientFactory.CreateClient();

            client.Timeout = TimeSpan.FromSeconds(5);
            var response = await client.GetAsync(monitoringParameters.Address, monitoringParameters.CancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}