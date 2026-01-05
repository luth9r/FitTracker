using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ReadModels;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Repositories;

internal sealed class WorkoutReadRepository(
    FitTrackerDbContext context,
    IMapper mapper) : IWorkoutReadRepository
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<Workout>> GetCompletedByUserIdAsync(Guid userId, CancellationToken cancelationToken)
    {
        var workoutsEf = await context.Workouts
            .AsNoTracking()
            .Where(w => w.UserId == userId && w.IsCompleted)
            .OrderBy(w => w.WorkoutDate)
            .ProjectTo<Workout>(mapper.ConfigurationProvider)
            .ToListAsync(cancelationToken);

        return workoutsEf;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<WorkoutSummary>> GetRecentByUserIdAsync(
        Guid userId,
        int take,
        CancellationToken cancellationToken)
    {
        var workoutsEf = await context.Workouts
            .AsNoTracking()
            .OrderByDescending(w => w.WorkoutDate)
            .Where(w => w.UserId == userId)
            .Take(take)
            .ProjectTo<WorkoutSummary>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return workoutsEf;
    }
}
