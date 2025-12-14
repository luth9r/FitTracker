using System;
using System.Collections.Generic;
using System.Text;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Abstract.Interfaces
{
    public interface IExerciseReadRepository
    {
        /// <summary>
        /// Retrieves exercises as a read-only collection based on the specified filter.
        /// </summary>
        /// <param name="filter">The type of exercises to retrieve <see cref="ExerciseFilterType"/>.
        /// </param>
        /// <param name="userId">
        /// The identifier of the user for filtering custom exercises; required when <paramref name="filter"/> is <see cref="ExerciseFilterType.Custom"/>.
        /// </param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A read-only list of exercises matching the specified filter.
        /// </returns>
        Task<IReadOnlyList<Exercise>> GetExercisesAsync(ExerciseFilterType filter, Guid? userId, CancellationToken cancellationToken);
    }
}
