using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Users;
using MediatR;

namespace FitTracker.Application.UseCases.User.Queries
{
    /// <summary>
    /// Query used to retrieve a limited number of recent workouts for a specific user.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user whose workouts are requested.</param>
    /// <param name="Take">The maximum number of recent workouts to return.</param>
    [ExcludeFromCodeCoverage]
    public sealed record GetRecentWorkoutsQuery(Guid UserId, int Take) : IRequest<Result<IReadOnlyList<RecentWorkoutResponse>>>;
}
