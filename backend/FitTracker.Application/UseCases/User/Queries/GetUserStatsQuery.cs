using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Users;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Queries
{
    /// <summary>
    /// Gets the user stats.
    /// </summary>
    /// <param name="UserId">The ID of the user.</param>
    /// <param name="PreferredUnits">The preferred units.</param>
    public record GetUserStatsQuery(Guid UserId, string PreferredUnits) : IRequest<Result<UserStatsResponse>>;
}
