using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Persistence.Repositories
{
    internal sealed class SetReadRepository(
        FitTrackerDbContext context,
        IMapper mapper) : ISetReadRepository
    {
        /// <inheritdoc/>
        public async Task<double> GetTotalWeightLiftedAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await context.Sets
                                .Where(s => s.WorkoutExercise!.Workout!.UserId == userId)
                                .SumAsync(
                                    s => s.WeightKg * s.Reps,
                                    cancellationToken);
        }
    }
}
