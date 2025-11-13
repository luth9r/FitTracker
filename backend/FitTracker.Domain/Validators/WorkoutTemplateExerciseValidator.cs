using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
    internal class WorkoutTemplateExerciseValidator : AbstractValidator<WorkoutTemplateExercise>
    {
        public WorkoutTemplateExerciseValidator()
        {
            Include(new BaseEntityValidator<WorkoutTemplateExercise>());

            #region Required Fields

            RuleFor(te => te.WorkoutTemplateId)
                .NotEmpty()
                .WithMessage("Template ID is required")
                .WithName("workoutTemplateId")
                .OverridePropertyName("workoutTemplateId");

            RuleFor(te => te.ExerciseId)
                .NotEmpty()
                .WithMessage("Exercise ID is required")
                .WithName("exerciseId")
                .OverridePropertyName("exerciseId");

            RuleFor(te => te.OrderIndex)
                .NotEmpty()
                .WithMessage("Order index is required")
                .WithName("orderIndex")
                .OverridePropertyName("orderIndex");

            #endregion

            // Detailed validations
            OrderValidation();
            NotesValidation();
        }

        private void OrderValidation()
        {
            RuleFor(te => te.OrderIndex)
                .GreaterThan(0)
                .WithMessage("Order index must be greater than 0")
                .LessThanOrEqualTo(1000)
                .WithMessage("Order index cannot exceed 1000")
                .WithName("orderIndex")
                .OverridePropertyName("orderIndex");
        }

        private void NotesValidation()
        {
            RuleFor(te => te.Notes)
                .MaximumLength(WorkoutTemplateExercise.NotesMaxLength)
                .When(te => !string.IsNullOrEmpty(te.Notes))
                .WithMessage($"Notes cannot exceed {WorkoutTemplateExercise.NotesMaxLength} characters")
                .WithName("notes")
                .OverridePropertyName("notes");
        }
    }
}

