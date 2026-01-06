using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public class MonitoringParameters
    {
        public string Address { get; set; } = string.Empty;
        public CancellationToken CancellationToken { get; set; }
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);
        public int? Port { get; set; }
    }
}