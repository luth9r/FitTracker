using FitTracker.Domain.Entities.TemplateAggregate;

namespace FitTracker.Domain.Abstract.Interfaces;

public interface IWorkoutTemplateWriteRepository
{
    /// <summary>
    ///     Adds a new workout template to the repository asynchronously.
    /// </summary>
    /// <param name="template">The workout template to be added.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(TemplateWorkout template, CancellationToken cancellationToken = default);
}
