using FitTracker.Domain.Entities;

namespace FitTracker.Domain.Abstract.Interfaces;

/// <summary>
///     Represents a repository for managing user entities.
/// </summary>
public interface IUserWriteRepository
{
    /// <summary>
    ///     Retrieves a user by their unique identifier in a read-only manner.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The user if found; otherwise, <c>null</c>.</returns>
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves a user by their email address in a read-only manner.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The user if found; otherwise, <c>null</c>.</returns>
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves a user by their Google authentication token in a read-only manner.
    /// </summary>
    /// <param name="token">The Google authentication token.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The user if found; otherwise, <c>null</c>.</returns>
    Task<User?> FindByGoogleTokenAsync(string token, CancellationToken cancellationToken);

    /// <summary>
    ///     Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing user entity in the repository.
    /// </summary>
    /// <param name="user">The user entity to update.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}
