using FitTracker.Domain.Entities;

namespace FitTracker.Domain.Abstract.Interfaces;

public interface IUserReadRepository
{
    /// <summary>
    ///     Retrieves a user by their username in a read-only manner.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The user if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetByUsernameReadonlyAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves a user by their email address in a read-only manner.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The user if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetByEmailReadonlyAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves a user by their unique identifier in a read-only manner.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The user if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetByIdReadonlyAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves a user by their Google authentication token in a read-only manner.
    /// </summary>
    /// <param name="token">The Google authentication token.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The user if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetByGoogleTokenReadonlyAsync(string token, CancellationToken cancellationToken);
}