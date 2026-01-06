using Facet;

using UpTulse.Application.MapperConfigs;
using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringTarget), nameof(MonitoringTarget.CreatedBy), nameof(MonitoringTarget.GroupId))]
    public partial class MonitoringTargetResponse
    {
        public MonitoringGroup? Group { get; set; }
    }
}