using System.ComponentModel.DataAnnotations;

using UpTulse.Core.Common;
using UpTulse.Core.Enums;

namespace UpTulse.Core.Entities
{
    public class MonitoringTarget : BaseEntity, IAuditedEntity
    {
        public string CreatedBy { get; set; } = default!;
        public string? Description { get; set; }

        public Guid GroupId { get; set; } = Guid.Empty;

        public MonitoringMethod Method { get; set; } = MonitoringMethod.Ping;

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Url { get; set; } = default!;
    }
}