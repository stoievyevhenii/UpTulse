using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringTarget), nameof(MonitoringTarget.CreatedBy), nameof(MonitoringTarget.Group), nameof(MonitoringTarget.Id))]
    public partial class MonitoringTargetRequest
    {
        public Guid? GroupId { get; set; }
    }
}