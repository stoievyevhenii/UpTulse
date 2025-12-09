using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringTarget),
        exclude: [nameof(MonitoringTarget.CreatedBy), nameof(MonitoringTarget.Id)],
        NullableProperties = true)]
    public partial class MonitoringTargetUpdateRequest
    {
    }
}