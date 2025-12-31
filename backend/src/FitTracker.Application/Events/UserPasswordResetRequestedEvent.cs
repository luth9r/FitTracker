using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace FitTracker.Application.Events
{
    /// <summary>
    /// Domain event raised when a user requests a password reset.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user requesting password reset.</param>
    /// <param name="Email">The email address where the reset link will be sent.</param>
    /// <param name="Username">The username of the user requesting password reset.</param>
    [ExcludeFromCodeCoverage]
    public sealed record UserPasswordResetRequestedEvent(Guid UserId, string Email, string Username) : INotification;
}
