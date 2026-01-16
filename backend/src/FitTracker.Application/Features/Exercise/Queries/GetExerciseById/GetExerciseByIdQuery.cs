using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using MediatR;

namespace FitTracker.Application.Features.Exercise.Queries.GetExerciseById;

/// <summary>
///     Represents a query to retrieve detailed information about a specific exercise for a given user.
/// </summary>
/// <param name="ExerciseId">The unique identifier of the exercise to retrieve.</param>
/// <param name="UserId">The unique identifier of the user for whom to retrieve the exercise.</param>
[ExcludeFromCodeCoverage]
public sealed record GetExerciseByIdQuery(Guid ExerciseId, Guid UserId) : IRequest<Result<ExerciseDetailsResponse>>;
