using System;
using System.Collections.Generic;
using System.Text;

using UpTulse.Application.Models;

namespace UpTulse.Application.Providers.MonitoringProtocolsFactory
{
    public abstract class MonitoringProtocolsProviderCreator
    {
        private IMonitoringProtocolsProvider _monitoringProtocolsProvider = default!;

        public abstract IMonitoringProtocolsProvider CreateMonitoringProtocolProvider();

        public Task<bool> PerformCheckAsync(MonitoringParameters monitoringParameters)
        {
            _monitoringProtocolsProvider ??= CreateMonitoringProtocolProvider();
            return _monitoringProtocolsProvider.PerformCheckAsync(monitoringParameters);
        }
    }
}