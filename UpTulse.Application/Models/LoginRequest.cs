using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Models
{
    public class LoginRequest
    {
        public string Password { get; set; } = default!;
        public string Username { get; set; } = default!;
    }
}