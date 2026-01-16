using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.Features.Exercise.Common;
using FitTracker.Domain.Enums;
using MediatR;

namespace FitTracker.Application.Features.Exercise.Queries.GetExercises;

/// <summary>
///     Query used to retrieve exercises for a specific user with a given filter type.
/// </summary>
/// <param name="Type">The filter that defines which exercises to return.</param>
/// <param name="UserId">The unique identifier of the user for whom exercises are requested.</param>
[ExcludeFromCodeCoverage]
public sealed record GetExercisesQuery(ExerciseFilterType Type, Guid UserId)
    : IRequest<Result<IReadOnlyList<ExerciseResponse>>>;
