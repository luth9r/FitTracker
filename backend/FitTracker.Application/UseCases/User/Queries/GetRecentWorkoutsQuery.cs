using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Users;
using MediatR;

namespace FitTracker.Application.UseCases.User.Queries
{
    public record GetRecentWorkoutsQuery(Guid UserId, int Take, string PreferredUnits) : IRequest<Result<IReadOnlyList<RecentWorkoutResponse>>>;
}
