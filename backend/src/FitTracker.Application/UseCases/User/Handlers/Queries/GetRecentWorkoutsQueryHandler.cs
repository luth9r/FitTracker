using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Users;
using FitTracker.Application.UseCases.User.Queries;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.UseCases.User.Handlers.Queries
{
    /// <summary>
    /// Handler for getting recent workouts for a user.
    /// </summary>
    /// <param name="workoutReadRepository">The <see cref="IWorkoutReadRepository"/>.</param>
    /// <param name="logger">The <see cref="ILogger{GetRecentWorkoutsQueryHandler}"/>.</param>
    /// <param name="mapper">The <see cref="IMapper"/>.</param>
    public sealed class GetRecentWorkoutsQueryHandler(
        IWorkoutReadRepository workoutReadRepository,
        ILogger<GetRecentWorkoutsQuery> logger,
        IMapper mapper)
        : IRequestHandler<GetRecentWorkoutsQuery, Result<IReadOnlyList<RecentWorkoutResponse>>>
    {
        /// <summary>
        /// Handles the get recent workouts query.
        /// </summary>
        /// <param name="request">The <see cref="GetRecentWorkoutsQuery"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The recent workouts list.</returns>
        public async Task<Result<IReadOnlyList<RecentWorkoutResponse>>> Handle(GetRecentWorkoutsQuery request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting GetRecentWorkouts process for UserId: {UserId} with Take: {Take}", request.UserId, request.Take);

            var workouts = await workoutReadRepository.GetRecentByUserIdAsync(request.UserId, request.Take, cancellationToken);

            var response = mapper.Map<IReadOnlyList<RecentWorkoutResponse>>(workouts);

            logger.LogInformation("GetRecentWorkouts process completed successfully for UserId: {UserId}. Workouts found: {Count}", request.UserId, response.Count);

            return Result.Success(response);
        }
    }
}
