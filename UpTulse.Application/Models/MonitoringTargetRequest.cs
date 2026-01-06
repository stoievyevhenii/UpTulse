using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringTarget), nameof(MonitoringTarget.CreatedBy))]
    public partial class MonitoringTargetRequest
    {
    }
}