using FitTracker.Domain.Entities.TemplateAggregate;

namespace FitTracker.Domain.Abstract.Interfaces;

public interface IWorkoutTemplateReadRepository
{
    /// <summary>
    ///     Retrieves a workout template by its name and user identifier asynchronously.
    /// </summary>
    /// <param name="name">The name of the workout template to retrieve.</param>
    /// <param name="userId">The unique identifier of the user who owns the workout template.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The workout template with the specified name for the given user, or null if no template is found.</returns>
    Task<TemplateWorkout?> GetTemplateByNameAsync(
        string name,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a workout template by its unique identifier and user identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the workout template to retrieve.</param>
    /// <param name="userId">The unique identifier of the user who owns the workout template.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The workout template with the specified identifier for the given user, or null if no template is found.</returns>
    Task<TemplateWorkout?> GetTemplateByIdAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);
}
