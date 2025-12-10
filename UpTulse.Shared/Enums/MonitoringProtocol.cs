using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UpTulse.Shared.Enums
{
    public enum MonitoringProtocol
    {
        [Description("HTTP")]
        HTTP = 0,

        [Description("Ping")]
        Ping = 1,
    }
}