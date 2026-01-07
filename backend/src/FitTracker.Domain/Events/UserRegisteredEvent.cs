using FitTracker.Domain.Abstract.Interfaces;

namespace FitTracker.Domain.Events;

/// <summary>
///     Domain event for user registration.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="Email">The user's email address.</param>
/// <param name="Username">The user's username.</param>
/// <param name="Culture">The culture to use for email localization.</param>
public sealed record UserRegisteredEvent(Guid UserId, string Email, string Username, string Culture)
    : IDomainEvent;
