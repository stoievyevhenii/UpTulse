using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringHistory), nameof(MonitoringGroup.CreatedBy), nameof(MonitoringGroup.Id))]
    public partial class MonitoringCreationHistoryRequest
    {
    }
}