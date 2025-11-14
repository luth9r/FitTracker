using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
