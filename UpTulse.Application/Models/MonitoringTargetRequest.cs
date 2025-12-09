using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringTarget), nameof(MonitoringTarget.CreatedBy), nameof(MonitoringTarget.Id))]
    public partial class MonitoringTargetRequest
    {
    }
}