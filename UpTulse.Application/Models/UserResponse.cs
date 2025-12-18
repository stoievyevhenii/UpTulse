using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public class UserResponse
    {
        public string FullName { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}