using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using MediatR;

namespace FitTracker.Application.Features.User.Queries.GetUserStats;

/// <summary>
///     Gets the user stats.
/// </summary>
/// <param name="UserId">The ID of the user.</param>
[ExcludeFromCodeCoverage]
public sealed record GetUserStatsQuery(Guid UserId) : IRequest<Result<UserStatsResponse>>;
