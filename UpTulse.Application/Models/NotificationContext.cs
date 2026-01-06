using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public class NotificationContext
    {
        public string Body { get; set; } = default!;
        public bool IsAvailabilityCritical { get; set; }
        public bool IsUnavailabilityCritical { get; set; }
        public bool IsUp { get; set; }
        public string Subject { get; set; } = default!;
    }
}