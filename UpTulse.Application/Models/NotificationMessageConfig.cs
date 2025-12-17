using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public class NotificationContext
    {
        public string Body { get; set; } = default!;
        public string Message { get; set; } = default!;
        public IReadOnlyList<string> Recipients { get; set; } = default!;
        public string Subject { get; set; } = default!;
    }
}