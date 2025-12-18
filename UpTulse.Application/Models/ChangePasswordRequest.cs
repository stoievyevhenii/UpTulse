using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}