using FitTracker.Domain.Entities;

namespace FitTracker.Domain.Abstract.Interfaces;

/// <summary>
///     Represents a repository for managing user entities.
/// </summary>
public interface IUserWriteRepository
{
    /// <summary>
    ///     Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    ///     Updates an existing user in the repository.
    /// </summary>
    /// <param name="user">The user entity to update.</param>
    void Update(User user);

    /// <summary>
    ///     Retrieves a user by their email address asynchronously.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the user entity if found, or null
    ///     if no user is associated with the specified email.
    /// </returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}