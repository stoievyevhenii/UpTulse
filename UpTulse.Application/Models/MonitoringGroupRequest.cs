using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringGroup), nameof(MonitoringGroup.CreatedBy), nameof(MonitoringGroup.Id))]
    public partial class MonitoringGroupRequest
    {
    }
}