using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringHistory),
        Include = [nameof(MonitoringHistory.MonitoringTargetId)]
    )]
    public partial class MonitoringTargetHistoryRequest
    {
    }
}