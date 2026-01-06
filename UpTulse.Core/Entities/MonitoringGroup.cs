using UpTulse.Core.Common;

namespace UpTulse.Core.Entities
{
    public class MonitoringGroup : BaseEntity, IAuditedEntity
    {
        public string CreatedBy { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}