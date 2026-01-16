using CSharpFunctionalExtensions;
using FitTracker.Application.Constants;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Constants;
using FluentValidation.Results;
using MediatR;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.Features.Exercise.Commands.CreateExercise;

/// <summary>
///     Handles the creation of a new exercise, ensuring no duplicates exist, storing optional image files, and persisting
///     the exercise to the database.
/// </summary>
/// <param name="readRepository">The exercise read repository.</param>
/// <param name="writeRepository">The exercise write repository.</param>
/// <param name="unitOfWork">The unit of work.</param>
/// <param name="blobStorageService">The blob storage service.</param>
public sealed class CreateExerciseCommandHandler(
    IExerciseReadRepository readRepository,
    IExerciseWriteRepository writeRepository,
    IUnitOfWork unitOfWork,
    IBlobStorageService blobStorageService) : IRequestHandler<CreateExerciseCommand, Result<Unit, ValidationResult>>
{
    /// <summary>
    ///     Handles the processing of the CreateExerciseCommand, which creates a new exercise for a specified user.
    /// </summary>
    /// <param name="request">
    ///     The command containing the data needed to create a new exercise, including the name, muscle group, equipment,
    ///     optional description, optional image, and the user ID.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token that propagates notification that the operation should be canceled.
    /// </param>
    /// <returns>
    ///     A Result object that represents either a successful operation returning a Unit value or a failure with a
    ///     ValidationResult
    ///     containing validation errors.
    /// </returns>
    public async Task<Result<Unit, ValidationResult>> Handle(
        CreateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var duplicateExercise = await readRepository.GetExerciseByName(
            request.Name,
            request.UserId,
            cancellationToken);

        if (duplicateExercise != null)
        {
            return ResultExtensions.ValidationFailure<Unit>(
                ErrorKeys.General,
                DomainErrors.Exercise.AlreadyExists);
        }

        string? imageUrl = null;

        if (request.Image != null)
        {
            imageUrl = await blobStorageService.UploadFileAsync(request.Image);
        }

        var exercise = Domain.Entities.Exercise.CreateCustom(
            request.UserId,
            request.Name,
            request.MuscleGroup,
            request.Equipment,
            request.Description,
            imageUrl);

        await writeRepository.AddAsync(exercise, cancellationToken);

        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result.Success<Unit, ValidationResult>(Unit.Value);
    }
}
