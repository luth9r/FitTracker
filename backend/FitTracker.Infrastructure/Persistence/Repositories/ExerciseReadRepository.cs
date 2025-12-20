using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Exceptions;
using FitTracker.Domain.ReadModels;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
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

        /// <inheritdoc/>
        public async Task<ExerciseDetails> GetExerciseDetailsAsync(Guid exerciseId, Guid userId, int fromDateMonths = 24, CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId is required.", nameof(userId));
            }

            var exerciseEf = await context.Exercises
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == exerciseId && (x.CreatedByUserId == null || x.CreatedByUserId == userId), cancellationToken)
                ?? throw new NotFoundException($"Exercise {exerciseId} not found", "EXERCISE_NOT_FOUND");

            var recordEf = await context.ExerciseRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ExerciseId == exerciseId && r.UserId == userId, cancellationToken);

            DateTime? filterThreshold = fromDateMonths > 0
                ? DateTime.UtcNow.Date.AddMonths(-fromDateMonths)
                : null;

            var volumeHistoryRaw = await (from w in context.Workouts.AsNoTracking()
                                          join we in context.WorkoutExercises on w.Id equals we.WorkoutId
                                          join s in context.Sets on we.Id equals s.WorkoutExerciseId
                                          where w.UserId == userId
                                             && we.ExerciseId == exerciseId
                                             && s.IsCompleted
                                             && (filterThreshold == null || w.WorkoutDate >= filterThreshold)
                                          group s by w.WorkoutDate.Date into g
                                          select new
                                          {
                                              Date = g.Key,
                                              SumValue = g.Sum(x => x.WeightKg * x.Reps),
                                          })
                                          .OrderBy(x => x.Date)
                                          .ToListAsync(cancellationToken);

            var volumeHistory = volumeHistoryRaw
                .Select(x => new ExerciseHistoryPoint(DateOnly.FromDateTime(x.Date), x.SumValue))
                .ToList();

            var hasRecords = recordEf != null && recordEf.TotalWorkouts > 0;
            double totalLifted = recordEf?.TotalLiftedKg ?? 0;
            int totalSets = recordEf?.TotalSets ?? 0;

            return new ExerciseDetails(
                Id: exerciseEf.Id,
                Name: exerciseEf.Name,
                MuscleGroup: (MuscleGroup)exerciseEf.MuscleGroup,
                Equipment: (Equipment)exerciseEf.Equipment,
                Description: exerciseEf.Description,
                ImageUrl: exerciseEf.ImageUrl,
                VideoUrl: exerciseEf.VideoUrl,
                IsCustom: exerciseEf.CreatedByUserId.HasValue,

                // PRs
                MaxWeightKg: recordEf?.MaxWeightKg ?? 0,
                MaxReps: recordEf?.MaxReps ?? 0,
                MaxVolume: recordEf?.MaxVolumeKg ?? 0,
                MaxTotalVolume: recordEf?.MaxTotalVolumeKg ?? 0,
                MaxWeightDate: hasRecords ? recordEf!.MaxWeightDate : null,
                MaxRepsDate: hasRecords ? recordEf!.MaxRepsDate : null,
                MaxVolumeDate: hasRecords ? recordEf!.MaxVolumeDate : null,
                MaxTotalVolumeDate: hasRecords ? recordEf!.MaxTotalVolumeDate : null,

                // Stats
                TotalWorkouts: recordEf?.TotalWorkouts ?? 0,
                TotalSets: totalSets,
                TotalReps: recordEf?.TotalReps ?? 0,
                TotalLifted: totalLifted,
                AvgWeightPerSet: totalSets > 0 ? totalLifted / totalSets : 0,
                AvgRepsPerSet: totalSets > 0 ? (double)(recordEf?.TotalReps ?? 0) / totalSets : 0,
                LastPerformed: hasRecords ? recordEf!.LastPerformed : null,
                VolumeHistory: volumeHistory);
        }
    }
}
