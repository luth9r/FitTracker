using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
    internal class WorkoutExerciseValidator : AbstractValidator<WorkoutExercise>
    {
        public WorkoutExerciseValidator()
        {
            #region Required Fields

            RuleFor(we => we.WorkoutId)
                .NotEmpty()
                .WithMessage("Workout ID is required")
                .WithName("workoutId")
                .OverridePropertyName("workoutId");

            RuleFor(we => we.ExerciseId)
                .NotEmpty()
                .WithMessage("Exercise ID is required")
                .WithName("exerciseId")
                .OverridePropertyName("exerciseId");

            RuleFor(we => we.OrderIndex)
                .NotEmpty()
                .WithMessage("Order index is required")
                .WithName("orderIndex")
                .OverridePropertyName("orderIndex");

            #endregion

            // Detailed validations
            OrderIndexValidation();
            NotesValidation();
        }

        private void OrderIndexValidation()
        {
            RuleFor(we => we.OrderIndex)
                .GreaterThan(0)
                .WithMessage("Order index must be greater than 0")
                .LessThanOrEqualTo(WorkoutExercise.MaxOrderIndex)
                .WithMessage($"Order index cannot exceed {WorkoutExercise.MaxOrderIndex}")
                .WithName("orderIndex")
                .OverridePropertyName("orderIndex");
        }

        private void NotesValidation()
        {
            RuleFor(we => we.Notes)
                .MaximumLength(WorkoutExercise.NotesMaxLength)
                .When(we => !string.IsNullOrEmpty(we.Notes))
                .WithMessage($"Notes cannot exceed {WorkoutExercise.NotesMaxLength} characters")
                .WithName("notes")
                .OverridePropertyName("notes");
        }
    }
}
