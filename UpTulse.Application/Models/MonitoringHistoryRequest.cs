using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringHistory),
    exclude: [nameof(MonitoringHistory.CreatedBy), nameof(MonitoringHistory.Id)],
    NullableProperties = true)]
    public partial class MonitoringHistoryRequest
    {
    }
}