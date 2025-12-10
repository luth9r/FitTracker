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
    public class GetRecentWorkoutsQueryHandler(
        IWorkoutReadRepository workoutReadRepository,
        ILogger<GetRecentWorkoutsQuery> logger,
        IMapper mapper)
        : IRequestHandler<GetRecentWorkoutsQuery, Result<IReadOnlyList<RecentWorkoutResponse>>>
    {
        public async Task<Result<IReadOnlyList<RecentWorkoutResponse>>> Handle(GetRecentWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await workoutReadRepository.GetRecentByUserIdAsync(request.UserId, request.Take, cancellationToken);

            var preferredUnits = request.PreferredUnits;

            var response = mapper.Map<IReadOnlyList<RecentWorkoutResponse>>(
                workouts,
                opt => opt.Items["preferredUnits"] = preferredUnits);

            return Result.Success(response);
        }
    }
}
