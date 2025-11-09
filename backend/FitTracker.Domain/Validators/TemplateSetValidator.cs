using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
    internal class TemplateSetValidator : AbstractValidator<TemplateSet>
    {
        public TemplateSetValidator()
        {
            #region Required Fields

            RuleFor(ts => ts.WorkoutTemplateExerciseId)
                .NotEmpty()
                .WithMessage("Template exercise ID is required")
                .WithName("workoutTemplateExerciseId")
                .OverridePropertyName("workoutTemplateExerciseId");

            RuleFor(ts => ts.SetNumber)
                .NotEmpty()
                .WithMessage("Set number is required")
                .WithName("setNumber")
                .OverridePropertyName("setNumber");

            RuleFor(ts => ts.PlannedWeight)
                .NotNull()
                .WithMessage("Planned weight is required")
                .WithName("plannedWeight")
                .OverridePropertyName("plannedWeight");

            RuleFor(ts => ts.PlannedReps)
                .NotEmpty()
                .WithMessage("Planned reps is required")
                .WithName("plannedReps")
                .OverridePropertyName("plannedReps");

            #endregion

            // Detailed validations
            SetNumberValidation();
            WeightValidation();
            RepsValidation();
            RestValidation();
        }

        private void SetNumberValidation()
        {
            RuleFor(ts => ts.SetNumber)
                .GreaterThan(0)
                .WithMessage("Set number must be greater than 0")
                .WithName("setNumber")
                .OverridePropertyName("setNumber");
        }

        private void WeightValidation()
        {
            RuleFor(ts => ts.PlannedWeight.ToKilograms())
                .GreaterThanOrEqualTo(0)
                .WithMessage("Planned weight cannot be negative")
                .LessThanOrEqualTo(TemplateSet.MaxWeightKg)
                .WithMessage($"Planned weight cannot exceed {TemplateSet.MaxWeightKg} kg")
                .WithName("plannedWeight")
                .OverridePropertyName("plannedWeight");
        }

        private void RepsValidation()
        {
            RuleFor(ts => ts.PlannedReps)
                .GreaterThan(0)
                .WithMessage("Planned reps must be greater than 0")
                .LessThanOrEqualTo(TemplateSet.MaxReps)
                .WithMessage($"Planned reps cannot exceed {TemplateSet.MaxReps}")
                .WithName("plannedReps")
                .OverridePropertyName("plannedReps");
        }

        private void RestValidation()
        {
            RuleFor(ts => ts.RestSeconds)
                .GreaterThanOrEqualTo(0)
                .When(ts => ts.RestSeconds.HasValue)
                .WithMessage("Rest seconds cannot be negative")
                .LessThanOrEqualTo(TemplateSet.MaxRestSeconds)
                .When(ts => ts.RestSeconds.HasValue)
                .WithMessage($"Rest cannot exceed {TemplateSet.MaxRestSeconds} seconds")
                .WithName("restSeconds")
                .OverridePropertyName("restSeconds");
        }
    }
}

