using System.ComponentModel.DataAnnotations;

using UpTulse.Core.Common;
using UpTulse.Shared.Enums;

namespace UpTulse.Core.Entities
{
    public class MonitoringTarget : BaseEntity, IAuditedEntity
    {
        [Required]
        public string Address { get; set; } = default!;

        public string CreatedBy { get; set; } = default!;
        public string? Description { get; set; }

        public Guid GroupId { get; set; } = Guid.Empty;

        public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);

        public bool IsAvailabilityCritical { get; set; } = false;
        public bool IsUnavailabilityCritical { get; set; } = false;

        [Required]
        public string Name { get; set; } = default!;

        public NotificationChannel NotificationChannel { get; set; } = NotificationChannel.None;
        public bool Paused { get; set; } = false;
        public MonitoringProtocol Protocol { get; set; } = MonitoringProtocol.Ping;
    }
}