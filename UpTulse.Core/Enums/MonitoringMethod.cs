using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UpTulse.Core.Enums
{
    public enum MonitoringMethod
    {
        [Description("HTTP")]
        HTTP = 0,

        [Description("Ping")]
        Ping = 1,
    }
}