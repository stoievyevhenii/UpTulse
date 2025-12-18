using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public class CreateUserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}