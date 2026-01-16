using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.Features.Exercise.Common;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.Features.Exercise.Queries.GetExercises;

/// <summary>
///     Handler for retrieving all exercises.
/// </summary>
/// <param name="readRepository">The <see cref="IExerciseReadRepository" />.</param>
/// <param name="mapper">The <see cref="IMapper" />.</param>
/// <param name="logger">The <see cref="ILogger{GetExerciseQueryHandler}" />.</param>
public sealed class GetExercisesQueryHandler(
    IExerciseReadRepository readRepository,
    IMapper mapper,
    ILogger<GetExercisesQueryHandler> logger)
    : IRequestHandler<GetExercisesQuery, Result<IReadOnlyList<ExerciseResponse>>>
{
    /// <summary>
    ///     Handles the get exercises query.
    /// </summary>
    /// <param name="request">The <see cref="GetExercisesQuery" />.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>
    ///     A result containing a read-only list of exercises.
    /// </returns>
    public async Task<Result<IReadOnlyList<ExerciseResponse>>> Handle(
        GetExercisesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Starting GetExercise query handling.");

        var exercisesResult = await readRepository.GetExercisesAsync(request.Type, request.UserId, cancellationToken);

        var response = mapper.Map<IReadOnlyList<ExerciseResponse>>(exercisesResult);

        logger.LogInformation("GetExercise query handling completed successfully.");

        return Result.Success(response);
    }
}
