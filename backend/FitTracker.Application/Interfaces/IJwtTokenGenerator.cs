using FitTracker.Domain.Entities;
using System.Security.Claims;

namespace FitTracker.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        string GenerateVerificationToken(User user);
        ClaimsPrincipal? ValidateToken(string token);

    }
}
