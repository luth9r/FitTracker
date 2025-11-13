using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth
{
    public class RegisterResponse
    {
        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string JWT { get; set; }

    }
}
