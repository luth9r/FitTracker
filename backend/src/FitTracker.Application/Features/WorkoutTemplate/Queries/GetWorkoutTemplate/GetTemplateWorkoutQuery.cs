using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using MediatR;

namespace FitTracker.Application.Features.WorkoutTemplate.Queries.GetWorkoutTemplate;

/// <summary>
///     Represents a query to retrieve a specific workout template for a user by template ID.
/// </summary>
/// <param name="TemplateId">The unique identifier of the workout template to retrieve.</param>
/// <param name="UserId">The unique identifier of the user who owns the workout template.</param>
[ExcludeFromCodeCoverage]
public sealed record GetTemplateWorkoutQuery(Guid TemplateId, Guid UserId)
    : IRequest<Result<GetTemplateWorkoutResponses>>;
