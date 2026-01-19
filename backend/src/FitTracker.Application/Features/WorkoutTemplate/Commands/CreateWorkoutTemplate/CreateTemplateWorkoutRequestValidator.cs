using FitTracker.Application.Constants;
using FitTracker.Domain.Entities.TemplateAggregate;
using FluentValidation;

namespace FitTracker.Application.Features.WorkoutTemplate.Commands.CreateWorkoutTemplate;

public class CreateTemplateWorkoutRequestValidator : AbstractValidator<CreateTemplateWorkoutRequest>
{
    public CreateTemplateWorkoutRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ValidationKeys.Template.NameRequired)
            .MaximumLength(TemplateWorkout.NameMaxLength)
            .WithMessage(ValidationKeys.Template.NameMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(TemplateWorkout.DescriptionMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleForEach(x => x.Exercises).SetValidator(new CreateTemplateExerciseDtoValidator());
    }
}

public class CreateTemplateExerciseDtoValidator : AbstractValidator<CreateTemplateExerciseDto>
{
    public CreateTemplateExerciseDtoValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotEmpty();

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(TemplateWorkoutExercise.MinOrderIndex)
            .LessThanOrEqualTo(TemplateWorkoutExercise.MaxOrderIndex);

        RuleForEach(x => x.Sets).SetValidator(new CreateTemplateSetDtoValidator());
    }
}

public class CreateTemplateSetDtoValidator : AbstractValidator<CreateTemplateSetDto>
{
    public CreateTemplateSetDtoValidator()
    {
        RuleFor(x => x.SetNumber).GreaterThan(0);

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .LessThanOrEqualTo(TemplateSet.MaxWeightKg);

        RuleFor(x => x.Reps)
            .GreaterThan(0)
            .LessThanOrEqualTo(TemplateSet.MaxReps);

        RuleFor(x => x.Type).IsInEnum();
    }
}
