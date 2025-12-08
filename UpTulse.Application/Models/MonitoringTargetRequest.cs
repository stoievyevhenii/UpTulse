using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringTarget), exclude: nameof(MonitoringTarget.CreatedBy))]
    public partial class MonitoringTargetRequest
    {
    }
}