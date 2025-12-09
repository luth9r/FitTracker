using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Persistence.Repositories
{
    internal sealed class WorkoutReadRepository(
        FitTrackerDbContext context,
        IMapper mapper,
        ILogger<UserReadRepository> logger) : IWorkoutReadRepository
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyList<Workout>> GetCompletedByUserAsync(Guid userId, CancellationToken cancelationToken)
        {
            var workoutsEf = await context.Workouts
                                    .AsNoTracking()
                                    .Where(w => w.UserId == userId && w.IsCompleted)
                                    .OrderBy(w => w.WorkoutDate)
                                    .ProjectTo<Workout>(mapper.ConfigurationProvider)
                                    .ToListAsync(cancelationToken);

            return workoutsEf;
        }
    }
}
