using System;
using System.Collections.Generic;
using System.Text;

using UpTulse.Application.Providers.MonitoringProtocolsFactory;
using UpTulse.Shared.Enums;

namespace UpTulse.Application.Services.Impl
{
    public class MonitoringProtocolResolver : IMonitoringProtocolResolver
    {
        public MonitoringProtocolsProviderCreator GetProtocolProvider(MonitoringProtocol monitoringProtocol)
        {
            throw new NotImplementedException();
        }
    }
}