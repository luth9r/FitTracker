using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using FitTracker.Application.Interfaces;
using FitTracker.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FitTracker.Infrastructure.Services;

public sealed class JwtTokenValidator(IOptions<JwtSettings> options) : IJwtTokenValidator
{
    private readonly JwtSettings settings = options.Value;

    /// <inheritdoc />
    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                },
                out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc />
    public Result<Guid> ValidatePurposeToken(string token, string expectedPurpose)
    {
        var principal = ValidateToken(token);
        if (principal == null)
        {
            return Result.Failure<Guid>("Invalid token");
        }

        var purposeClaim = principal.FindFirst("purpose");
        if (purposeClaim == null || purposeClaim.Value != expectedPurpose)
        {
            return Result.Failure<Guid>("Invalid token purpose");
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Result.Failure<Guid>("Invalid user ID");
        }

        return Result.Success(userId);
    }
}