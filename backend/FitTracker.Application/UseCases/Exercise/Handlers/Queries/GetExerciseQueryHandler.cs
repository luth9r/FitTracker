using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Exercise;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.Exercise.Queries;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.UseCases.Exercise.Handlers.Queries
{
    /// <summary>
    /// Handler for retrieving all exercises.
    /// </summary>
    /// <param name="readRepository">The <see cref="IExerciseReadRepository"/>.</param>
    /// <param name="mapper">The <see cref="IMapper"/>.</param>
    /// <param name="logger">The <see cref="ILogger{GetExerciseQueryHandler}"/>.</param>
    public sealed class GetExerciseQueryHandler(
        IExerciseReadRepository readRepository,
        IMapper mapper,
        ILocalizationService localization,
        ILogger<GetExerciseQueryHandler> logger) : IRequestHandler<GetExerciseQuery, Result<IReadOnlyList<ExerciseResponse>>>
    {
        /// <summary>
        /// Handles the get exercises query.
        /// </summary>
        /// <param name="request">The <see cref="GetExerciseQuery"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>
        /// A result containing a read-only list of exercises.
        /// </returns>
        public async Task<Result<IReadOnlyList<ExerciseResponse>>> Handle(GetExerciseQuery request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting GetExercise query handling.");

            var exercisesResult = await readRepository.GetExercisesAsync(request.Type, request.UserId, cancellationToken);

            var response = mapper.Map<IReadOnlyList<ExerciseResponse>>(exercisesResult)
                .Select(x => x with
                {
                    MuscleGroup = localization.GetString($"Exercise.MuscleGroup.{x.MuscleGroup}"),
                    Equipment = localization.GetString($"Exercise.Equipment.{x.Equipment}")
                })
                .ToList();

            logger.LogInformation("GetExercise query handling completed successfully.");

            return Result.Success<IReadOnlyList<ExerciseResponse>>(response);
        }
    }
}
