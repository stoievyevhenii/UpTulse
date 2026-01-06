using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public class LoginResponse
    {
        public string Email { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
    }
}