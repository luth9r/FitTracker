using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using MediatR;

namespace FitTracker.Application.Events
{
    /// <summary>
    /// Domain event published when a new user successfully completes registration.
    /// </summary>
    /// <param name="UserId">Unique identifier of the newly registered user.</param>
    /// <param name="Email">User's email address for sending verification email.</param>
    /// <param name="Username">User's display name for personalized email greeting.</param>
    [ExcludeFromCodeCoverage]
    public sealed record UserRegisteredEvent(Guid UserId, string Email, string Username) : INotification;
}
