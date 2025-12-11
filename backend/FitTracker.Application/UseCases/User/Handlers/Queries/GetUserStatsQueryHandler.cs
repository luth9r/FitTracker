using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Users;
using FitTracker.Application.UseCases.User.Queries;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.UseCases.User.Handlers.Queries
{
    /// <summary>
    /// Handler for processing get user stats queries.
    /// </summary>
    /// <param name="workoutReadRepository">The <see cref="IWorkoutReadRepository"/>.</param>
    /// <param name="setReadRepository">The <see cref="ISetReadRepository"/>.</param>
    /// <param name="logger">The <see cref="ILogger{GetUserStatsQueryHandler}"/>.</param>
    public class GetUserStatsQueryHandler(
        IWorkoutReadRepository workoutReadRepository,
        ISetReadRepository setReadRepository,
        ILogger<GetUserStatsQueryHandler> logger
        ) : IRequestHandler<GetUserStatsQuery, Result<UserStatsResponse>>
    {
        /// <summary>
        /// Handles the get user stats query.
        /// </summary>
        /// <param name="request">The <see cref="GetUserStatsQuery"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Result{UserStatsResponse}"/>.</returns>
        public async Task<Result<UserStatsResponse>> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting get user stats process for user: {UserId}", request.UserId);

            UnitSystem preferredUnits;
            try
            {
                preferredUnits = UnitSystem.FromString(request.PreferredUnits);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Invalid unit system: {Unit}", request.PreferredUnits);
                return Result.Failure<UserStatsResponse>("INVALID_UNIT_SYSTEM");
            }

            var workouts = await workoutReadRepository.GetCompletedByUserIdAsync(request.UserId, cancellationToken);

            if (workouts.Count == 0)
            {
                logger.LogWarning("No workouts found for user: {UserId}", request.UserId);
                return Result.Success(new UserStatsResponse(0, 0, 0, 0));
            }

            var orderedDates = workouts
                                .Select(w => w.WorkoutDate.Date)
                                .OrderBy(d => d)
                                .ToList();

            var totalWorkouts = orderedDates.Count;
            var trainingDays = (orderedDates[^1] - orderedDates[0]).Days + 1;
            var longestStreak = CalculateLongestStreak(orderedDates, maxGapDays: 3);

            var totalWeightKg = await setReadRepository.GetTotalWeightLiftedAsync(request.UserId, cancellationToken);

            var totalWeightUserUnits = UnitSystem.ConvertFromMetric(totalWeightKg, preferredUnits);
            var roundedTotalWeight = (double)Math.Round(totalWeightUserUnits, 1);

            return Result.Success(new UserStatsResponse(totalWorkouts, trainingDays, longestStreak, roundedTotalWeight));
        }

        /// <summary>
        /// Calculates the longest streak of workouts.
        /// </summary>
        /// <param name="dates">The list of workout dates.</param>
        /// <param name="maxGapDays">The maximum gap in days between workouts.</param>
        /// <returns>The longest streak of workouts.</returns>
        private static int CalculateLongestStreak(IReadOnlyList<DateTime> dates, int maxGapDays)
        {
            if (dates.Count == 0)
            {
                return 0;
            }

            var longest = 1;
            var current = 1;

            for (int i = 1; i < dates.Count; i++)
            {
                var diff = (dates[i] - dates[i - 1]).TotalDays;
                if (diff <= maxGapDays)
                {
                    current++;
                }
                else
                {
                    if (current > longest)
                    {
                        longest = current;
                    }

                    current = 1;
                }
            }

            return current > longest ? current : longest;
        }
    }
}
