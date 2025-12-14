using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Repositories
{
    internal sealed class ExerciseReadRepository(
        FitTrackerDbContext context,
        IMapper mapper) : IExerciseReadRepository
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyList<Exercise>> GetExercisesAsync(ExerciseFilterType filter, Guid? userId, CancellationToken cancellationToken)
        {
            var query = context.Exercises
                            .AsNoTracking()
                            .AsQueryable();

            switch (filter)
            {
                case ExerciseFilterType.Standard:
                    query = query.Where(x => x.CreatedByUserId == null);
                    break;

                case ExerciseFilterType.Custom:
                    if (!userId.HasValue || userId.Value == Guid.Empty)
                    {
                        throw new ArgumentException("UserId is required when filter is Custom.", nameof(userId));
                    }

                    query = query.Where(x => x.CreatedByUserId == userId);
                    break;

                default:
                    if (!userId.HasValue || userId.Value == Guid.Empty)
                    {
                        throw new ArgumentException("UserId is required when filter is All.", nameof(userId));
                    }

                    query = query.Where(x =>
                        x.CreatedByUserId == null || x.CreatedByUserId == userId);
                    break;
            }

            var exercises = await query
                .ProjectTo<Exercise>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return exercises;
        }
    }
}
