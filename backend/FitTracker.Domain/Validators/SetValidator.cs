// FitTracker.Domain/Entities/Validators/SetValidator.cs
using FluentValidation;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Validators
{
    internal class SetValidator : AbstractValidator<Set>
    {
        public SetValidator()
        {
            Include(new BaseEntityValidator<Set>());

            #region WorkoutExerciseId

            RuleFor(s => s.WorkoutExerciseId)
                .NotEmpty()
                .WithMessage("Workout exercise ID is required")
                .WithName("workoutExerciseId")
                .OverridePropertyName("workoutExerciseId");

            #endregion

            #region SetNumber

            RuleFor(s => s.SetNumber)
                .NotEmpty()
                .WithMessage("Set number is required")
                .WithName("setNumber")
                .OverridePropertyName("setNumber");

            #endregion

            #region Weight

            RuleFor(s => s.Weight)
                .NotNull()
                .WithMessage("Weight is required")
                .WithName("weight")
                .OverridePropertyName("weight");

            #endregion

            #region Reps

            RuleFor(s => s.Reps)
                .NotEmpty()
                .WithMessage("Reps is required")
                .WithName("reps")
                .OverridePropertyName("reps");

            #endregion

            #region SetType

            RuleFor(s => s.SetType)
                .NotNull()
                .WithMessage("Set type is required")
                .WithName("setType")
                .OverridePropertyName("setType");

            #endregion

            // Detailed validations
            SetNumberValidation();
            WeightValidation();
            RepsValidation();
            RestSecondsValidation();
            SetTypeValidation();
        }

        private void SetNumberValidation()
        {
            RuleFor(s => s.SetNumber)
                .GreaterThan(0)
                .WithMessage("Set number must be greater than 0")
                .WithName("setNumber")
                .OverridePropertyName("setNumber");
        }

        private void WeightValidation()
        {
            RuleFor(s => s.Weight.ToKilograms())
                .GreaterThan(0)
                .WithMessage("Weight must be greater than 0")
                .LessThanOrEqualTo(Set.MaxWeightKg)
                .WithMessage($"Weight cannot exceed {Set.MaxWeightKg} kg")
                .WithName("weight")
                .OverridePropertyName("weight");
        }

        private void RepsValidation()
        {
            RuleFor(s => s.Reps)
                .GreaterThan(0)
                .WithMessage("Reps must be greater than 0")
                .LessThanOrEqualTo(Set.MaxReps)
                .WithMessage($"Reps cannot exceed {Set.MaxReps}")
                .WithName("reps")
                .OverridePropertyName("reps");
        }

        private void RestSecondsValidation()
        {
            RuleFor(s => s.RestSeconds)
                .GreaterThanOrEqualTo(0)
                .When(s => s.RestSeconds.HasValue)
                .WithMessage("Rest seconds cannot be negative")
                .LessThanOrEqualTo(Set.MaxRestSeconds)
                .When(s => s.RestSeconds.HasValue)
                .WithMessage($"Rest cannot exceed {Set.MaxRestSeconds} seconds")
                .WithName("restSeconds")
                .OverridePropertyName("restSeconds");
        }

        private void SetTypeValidation()
        {
            RuleFor(s => s.SetType)
                .IsInEnum()
                .WithMessage("Set type must be a valid value")
                .WithName("setType")
                .OverridePropertyName("setType");
        }

    }
}
