using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace FitTracker.Application.Events
{
    /// <summary>
    /// Domain event published when a user explicitly requests a resend of their email verification link.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user requesting verification.</param>
    /// <param name="Email">The user's email address where the verification link will be sent.</param>
    /// <param name="Username">The user's username for personalized email content.</param>
    [ExcludeFromCodeCoverage]
    public sealed record UserRequestedVerificationEvent(Guid UserId, string Email, string Username) : INotification;
}
