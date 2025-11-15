using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    public class CompleteGoogleRegistrationRequest
    {
        public string IdToken { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
