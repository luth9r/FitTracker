using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Exercise;
using MediatR;

namespace FitTracker.Application.UseCases.Exercise.Queries
{
    /// <summary>
    /// Represents a query to retrieve detailed information about a specific exercise for a given user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record GetExerciseByIdQuery(Guid exerciseId, Guid UserId) : IRequest<Result<ExerciseDetailsResponse>>;
}
