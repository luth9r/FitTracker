using FitTracker.Domain.Abstract;

namespace FitTracker.Domain.Events;

/// <summary>
///     Represents a domain event triggered when a user requests a password reset.
/// </summary>
/// <param name="UserId">The unique identifier of the user requesting password reset.</param>
/// <param name="Email">The email address where the reset link will be sent.</param>
/// <param name="Username">The username of the user requesting password reset.</param>
/// <param name="Culture">The culture to use for email localization.</param>
public sealed record UserRequestedPasswordResetEvent(
    Guid UserId,
    string Email,
    string Username,
    string Culture)
    : DomainEvent
{
    public static UserRequestedPasswordResetEvent Create(Guid userId, string email, string username, string culture)
    {
        return new UserRequestedPasswordResetEvent(userId, email, username, culture)
        {
            CorrelationId = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
        };
    }
}
