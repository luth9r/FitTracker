using FitTracker.Domain.Entities;

namespace FitTracker.Domain.Abstract.Interfaces;

/// <summary>
///     Provides write operations for managing exercise entities in the system.
/// </summary>
public interface IExerciseWriteRepository
{
    Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default);

    Task UpdateAsync(Exercise exercise);
}
