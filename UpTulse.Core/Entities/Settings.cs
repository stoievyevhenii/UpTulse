using UpTulse.Core.Common;
using UpTulse.Core.Enums;

namespace UpTulse.Core.Entities
{
    public class Settings : BaseEntity, IAuditedEntity
    {
        public string CreatedBy { get; set; } = default!;
        public SettingKey Name { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}