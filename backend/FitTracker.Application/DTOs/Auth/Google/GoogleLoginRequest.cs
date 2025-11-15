using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    public class GoogleLoginRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }
}
