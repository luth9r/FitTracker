using FitTracker.Application.DTOs.Auth.Google;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.Interfaces
{
    public interface IGoogleTokenValidator
    {
        Task<GoogleTokenPayload?> ValidateAsync(string idToken);
    }
}
