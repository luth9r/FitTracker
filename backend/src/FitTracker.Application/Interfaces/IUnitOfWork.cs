namespace FitTracker.Application.Interfaces;

/// <summary>
///     Unit of Work interface for managing transactions.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Saves all changes made in this unit of work.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
