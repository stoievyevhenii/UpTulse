using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringGroup), exclude: nameof(MonitoringGroup.CreatedBy))]
    public partial class MonitoringGroupResponse
    {
    }
}