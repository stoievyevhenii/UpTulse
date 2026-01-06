using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringGroup), exclude: [nameof(MonitoringGroup.CreatedBy), nameof(MonitoringGroup.Id)], NullableProperties = true)]
    public partial class MonitoringGroupUpdateRequest
    {
    }
}