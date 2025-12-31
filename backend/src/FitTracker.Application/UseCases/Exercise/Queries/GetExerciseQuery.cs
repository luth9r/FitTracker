using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Exercise;
using FitTracker.Domain.Enums;
using MediatR;

namespace FitTracker.Application.UseCases.Exercise.Queries
{
    /// <summary>
    /// Query used to retrieve exercises for a specific user with a given filter type.
    /// </summary>
    /// <param name="Type">The filter that defines which exercises to return.</param>
    /// <param name="UserId">The unique identifier of the user for whom exercises are requested.</param>
    public sealed record GetExerciseQuery(ExerciseFilterType Type, Guid UserId) : IRequest<Result<IReadOnlyList<ExerciseResponse>>>;
}
