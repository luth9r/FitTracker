using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ReadModels;

namespace FitTracker.Domain.Abstract.Interfaces;

public interface IExerciseReadRepository
{
    /// <summary>
    ///     Retrieves exercises as a read-only collection based on the specified filter.
    /// </summary>
    /// <param name="filter">
    ///     The type of exercises to retrieve <see cref="ExerciseFilterType" />.
    /// </param>
    /// <param name="userId">
    ///     The identifier of the user for filtering custom exercises; required when <paramref name="filter" /> is
    ///     <see cref="ExerciseFilterType.Custom" />.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A read-only list of exercises matching the specified filter.
    /// </returns>
    Task<IReadOnlyList<Exercise>> GetExercisesAsync(
        ExerciseFilterType filter,
        Guid? userId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves comprehensive details for a specific exercise, including user-specific personal records, aggregate
    ///     statistics, and volume history filtered by a date range.
    /// </summary>
    /// <param name="exerciseId">The unique identifier of the exercise.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="fromDateMonths">The number of moths back from today to include in the volume history (0 for all time).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ExerciseDetails" /> object containing metadata, PRs, and progress history.</returns>
    Task<ExerciseDetails> GetExerciseDetailsAsync(
        Guid exerciseId,
        Guid userId,
        int fromDateMonths = 24,
        CancellationToken cancellationToken = default);
}