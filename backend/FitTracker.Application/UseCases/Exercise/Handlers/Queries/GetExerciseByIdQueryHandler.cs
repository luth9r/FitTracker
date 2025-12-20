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
    /// Handles <see cref="GetExerciseByIdQuery"/> requests by loading exercise details from the read repository and mapping them to an <see cref="ExerciseDetailsResponse"/>.
    /// </summary>
    /// <param name="exerciseReadRepository">Read-only repository used to retrieve exercise details.</param>
    /// <param name="mapper">AutoMapper instance used to map domain models to response DTOs.</param>
    public class GetExerciseByIdQueryHandler(
        IExerciseReadRepository exerciseReadRepository,
        ILocalizationService localization,
        IMapper mapper) : IRequestHandler<GetExerciseByIdQuery, Result<ExerciseDetailsResponse>>
    {
        /// <summary>
        /// Processes the query and returns detailed information about the requested exercise.
        /// </summary>
        /// <param name="request">The query containing the exercise identifier and user identifier.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>
        /// A successful result containing <see cref="ExerciseDetailsResponse"/> when the exercise is found;
        /// otherwise, a failed result describing the error.
        /// </returns>
        public async Task<Result<ExerciseDetailsResponse>> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
        {
            var exercise = await exerciseReadRepository.GetExerciseDetailsAsync(request.exerciseId, request.UserId, cancellationToken: cancellationToken);

            var result = mapper.Map<ExerciseDetailsResponse>(exercise) with
            {
                Name = exercise.IsCustom
                        ? exercise.Name
                        : localization.GetString($"Exercise.Name.{exercise.Name}"),

                MuscleGroup = localization.GetString($"Exercise.MuscleGroup.{exercise.MuscleGroup}"),
                Equipment = localization.GetString($"Exercise.Equipment.{exercise.Equipment}")
            };

            return Result.Success(result);
        }
    }
}
