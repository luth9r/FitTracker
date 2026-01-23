namespace FitTracker.Application.Interfaces;

/// <summary>
///     Unit of Work interface for managing transactions.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Saves all changes made in this unit of work.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The number of objects written to the underlying database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
