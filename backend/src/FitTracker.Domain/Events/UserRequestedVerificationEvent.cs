using FitTracker.Domain.Abstract;

namespace FitTracker.Domain.Events;

/// <summary>
///     Represents a domain event triggered when a user requests email verification.
/// </summary>
/// <param name="UserId">The unique identifier of the user requesting verification.</param>
/// <param name="Email">The email address of the user requesting verification.</param>
/// <param name="Username">The username of the user requesting verification.</param>
/// <param name="Culture">The culture to use for email localization.</param>
public sealed record UserRequestedVerificationEvent(
    Guid UserId,
    string Email,
    string Username,
    string Culture)
    : DomainEvent
{
    public static UserRequestedVerificationEvent Create(Guid userId, string email, string username, string culture)
    {
        return new UserRequestedVerificationEvent(userId, email, username, culture)
        {
            CorrelationId = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
        };
    }
}
