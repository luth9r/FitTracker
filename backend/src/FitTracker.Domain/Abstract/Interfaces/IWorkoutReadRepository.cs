using FitTracker.Domain.Entities;
using FitTracker.Domain.ReadModels;

namespace FitTracker.Domain.Abstract.Interfaces;

/// <summary>
///     Repository for reading workout data.
/// </summary>
public interface IWorkoutReadRepository
{
    /// <summary>
    ///     Gets the completed workouts by user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancelationToken">The cancellation token.</param>
    /// <returns>The completed workouts by the user.</returns>
    Task<IReadOnlyList<Workout>> GetCompletedByUserIdAsync(Guid userId, CancellationToken cancelationToken);

    /// <summary>
    ///     Retrieves a limited list of recent workouts for the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose workouts are requested.</param>
    /// <param name="take">The maximum number of recent workouts to return.</param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A read-only list of recent workout summaries.
    /// </returns>
    Task<IReadOnlyList<WorkoutSummary>> GetRecentByUserIdAsync(
        Guid userId,
        int take,
        CancellationToken cancellationToken);
}