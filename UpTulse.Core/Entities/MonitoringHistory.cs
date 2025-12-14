using UpTulse.Core.Common;

namespace UpTulse.Core.Entities
{
    public class MonitoringHistory : BaseEntity, IAuditedEntity
    {
        public string CreatedBy { get; set; } = default!;
        public DateTimeOffset EndTimeStamp { get; set; }
        public bool IsUp { get; set; } = false;
        public Guid MonitoringTargetId { get; set; }
        public float ResponseTimeInMs { get; set; } = 0;
        public DateTimeOffset StartTimeStamp { get; set; }
    }
}