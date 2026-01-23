using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.Constants;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities.TemplateAggregate;
using FluentValidation.Results;
using MediatR;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.Features.WorkoutTemplate.Commands.CreateWorkoutTemplate;

public sealed class CreateTemplateWorkoutCommandHandler(
    IWorkoutTemplateReadRepository readRepository,
    IWorkoutTemplateWriteRepository writeRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateTemplateWorkoutCommand, Result<Unit, ValidationResult>>
{
    public async Task<Result<Unit, ValidationResult>> Handle(
        CreateTemplateWorkoutCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Check for duplicate template name for this user
        var duplicateTemplate = await readRepository.FindTemplateByNameReadonlyAsync(
            request.Name,
            request.UserId,
            cancellationToken);

        if (duplicateTemplate != null)
        {
            return ResultExtensions.ValidationFailure<Unit>(
                ErrorKeys.General,
                "A template with this name already exists."); // Или вынести в DomainErrors.Template.AlreadyExists
        }

        // 2. Create the Aggregate Root
        var template = TemplateWorkout.Create(
            request.UserId,
            request.Name,
            request.Description);

        // 3. Add Exercises and Sets
        if (request.Exercises != null)
        {
            foreach (var exDto in request.Exercises)
            {
                var domainSets = mapper.Map<List<TemplateSetData>>(exDto.Sets);

                template.AddExercise(
                    exDto.ExerciseId,
                    exDto.OrderIndex,
                    exDto.Notes,
                    domainSets);
            }
        }

        // 4. Persist
        await writeRepository.AddAsync(template, cancellationToken);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result.Success<Unit, ValidationResult>(Unit.Value);
    }
}
