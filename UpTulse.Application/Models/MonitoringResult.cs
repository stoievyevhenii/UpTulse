using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public record MonitoringResult(
        string Name,
        bool IsUp,
        long ResponseTimeMs,
        DateTime Timestamp
    );
}