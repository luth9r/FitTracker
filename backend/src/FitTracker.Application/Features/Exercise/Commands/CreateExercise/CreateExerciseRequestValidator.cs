using FitTracker.Application.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FitTracker.Application.Features.Exercise.Commands.CreateExercise;

public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ValidationKeys.Exercise.NameRequired)
            .MaximumLength(Domain.Entities.Exercise.NameMaxLength)
            .WithMessage(ValidationKeys.Exercise.NameLength);
        RuleFor(x => x.MuscleGroup)
            .IsInEnum()
            .WithMessage(ValidationKeys.Exercise.MuscleGroupNotValid);
        RuleFor(x => x.Equipment)
            .IsInEnum()
            .WithMessage(ValidationKeys.Exercise.EquipmentNotValid);
        RuleFor(x => x.Description)
            .MaximumLength(Domain.Entities.Exercise.DescriptionMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage(ValidationKeys.Exercise.DescriptionLength);
        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length <= 5 * 1024 * 1024) // 5MB
            .WithMessage(ValidationKeys.Exercise.FileSizeTooLarge)
            .Must(file => file == null || IsValidImageType(file))
            .WithMessage(ValidationKeys.Exercise.InvalidImageType);
    }

    private bool IsValidImageType(IFormFile file)
    {
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
        return allowedTypes.Contains(file.ContentType.ToLower());
    }
}
