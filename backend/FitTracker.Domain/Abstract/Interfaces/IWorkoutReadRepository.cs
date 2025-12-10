using System;
using System.Collections.Generic;
using System.Text;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ReadModels;

namespace FitTracker.Domain.Abstract.Interfaces
{
    /// <summary>
    /// Repository for reading workout data.
    /// </summary>
    public interface IWorkoutReadRepository
    {
        /// <summary>
        /// Gets the completed workouts by user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancelationToken">The cancellation token.</param>
        /// <returns>The completed workouts by the user.</returns>
        Task<IReadOnlyList<Workout>> GetCompletedByUserIdAsync(Guid userId, CancellationToken cancelationToken);

        Task<IReadOnlyList<WorkoutSummary>> GetRecentByUserIdAsync(Guid userId, int take, CancellationToken cancellationToken);
    }
}
