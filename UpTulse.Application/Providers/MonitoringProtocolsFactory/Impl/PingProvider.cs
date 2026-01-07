using System.Net.NetworkInformation;

using UpTulse.Application.Models;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory.Impl
{
    public class PingProvider : IMonitoringProtocolsProvider
    {
        public async Task<bool> PerformCheckAsync(MonitoringParameters monitoringParameters)
        {
            try
            {
                using var ping = new Ping();

                var reply = await ping
                    .SendPingAsync(hostNameOrAddress: monitoringParameters.Address,
                                   timeout: monitoringParameters.Interval,
                                   cancellationToken: monitoringParameters.CancellationToken);

                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}