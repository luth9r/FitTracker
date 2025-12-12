using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO representing the payload of a Google authentication token.
    /// </summary>
    /// <param name="GoogleId">The unique identifier for the Google user.</param>
    /// <param name="Email">The email address of the Google user.</param>
    /// <param name="FirstName">The first name of the Google user.</param>
    /// <param name="LastName">The last name of the Google user.</param>
    [ExcludeFromCodeCoverage]
    public sealed record GoogleTokenPayload(string GoogleId, string Email, string FirstName, string LastName);
}
