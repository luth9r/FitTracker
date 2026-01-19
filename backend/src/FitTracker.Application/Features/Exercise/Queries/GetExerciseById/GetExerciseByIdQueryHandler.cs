using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.Constants;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;

namespace FitTracker.Application.Features.Exercise.Queries.GetExerciseById;

/// <summary>
///     Handles <see cref="GetExerciseByIdQuery" /> requests by loading exercise details from the read repository and
///     mapping them to an <see cref="ExerciseDetailsResponse" />.
/// </summary>
/// <param name="exerciseReadRepository">Read-only repository used to retrieve exercise details.</param>
/// <param name="mapper">AutoMapper instance used to map domain models to response DTOs.</param>
public sealed class GetExerciseByIdQueryHandler(
    IExerciseReadRepository exerciseReadRepository,
    IMapper mapper) : IRequestHandler<GetExerciseByIdQuery, Result<ExerciseDetailsResponse>>
{
    /// <summary>
    ///     Processes the query and returns detailed information about the requested exercise.
    /// </summary>
    /// <param name="request">The query containing the exercise identifier and user identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    ///     A successful result containing <see cref="ExerciseDetailsResponse" /> when the exercise is found;
    ///     otherwise, a failed result describing the error.
    /// </returns>
    public async Task<Result<ExerciseDetailsResponse>> Handle(
        GetExerciseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var exercise = await exerciseReadRepository.GetExerciseDetailsAsync(
            request.ExerciseId,
            request.UserId,
            cancellationToken: cancellationToken);

        if (exercise is null)
        {
            return Result.Failure<ExerciseDetailsResponse>(ErrorKeys.NotFound);
        }

        var result = mapper.Map<ExerciseDetailsResponse>(exercise);

        return Result.Success(result);
    }
}
