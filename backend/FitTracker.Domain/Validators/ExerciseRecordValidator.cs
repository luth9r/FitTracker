using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
    internal class ExerciseRecordValidator : AbstractValidator<ExerciseRecord>
    {
        public ExerciseRecordValidator()
        {
            Include(new BaseEntityValidator<ExerciseRecord>());

            #region Required Fields

            RuleFor(er => er.UserId)
                .NotEmpty()
                .WithMessage("User ID is required")
                .WithName("userId")
                .OverridePropertyName("userId");

            RuleFor(er => er.ExerciseId)
                .NotEmpty()
                .WithMessage("Exercise ID is required")
                .WithName("exerciseId")
                .OverridePropertyName("exerciseId");

            RuleFor(er => er.MaxWeight)
                .NotNull()
                .WithMessage("Max weight is required")
                .WithName("maxWeight")
                .OverridePropertyName("maxWeight");

            #endregion

            // Detailed validations
            WeightValidation();
            RepsValidation();
            VolumeValidation();
            StatsValidation();
            DateValidation();
        }

        private void WeightValidation()
        {
            RuleFor(er => er.MaxWeight)
                .Must(w => w.ToKilograms() >= 0)
                .WithMessage("Max weight cannot be negative")
                .Must(w => w.ToKilograms() <= 3000)
                .WithMessage("Max weight cannot exceed 3000 kg")
                .WithName("maxWeight")
                .OverridePropertyName("maxWeight");
        }

        private void RepsValidation()
        {
            RuleFor(er => er.MaxReps)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max reps cannot be negative")
                .LessThanOrEqualTo(1000)
                .WithMessage("Max reps cannot exceed 1000")
                .WithName("maxReps")
                .OverridePropertyName("maxReps");
        }

        private void VolumeValidation()
        {
            RuleFor(er => er.MaxVolume)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max volume cannot be negative")
                .WithName("maxVolume")
                .OverridePropertyName("maxVolume");

            RuleFor(er => er.MaxTotalVolume)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max total volume cannot be negative")
                .WithName("maxTotalVolume")
                .OverridePropertyName("maxTotalVolume");
        }

        private void StatsValidation()
        {
            RuleFor(er => er.TotalWorkouts)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total workouts cannot be negative")
                .WithName("totalWorkouts")
                .OverridePropertyName("totalWorkouts");

            RuleFor(er => er.TotalSets)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total sets cannot be negative")
                .WithName("totalSets")
                .OverridePropertyName("totalSets");

            RuleFor(er => er.TotalReps)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total reps cannot be negative")
                .WithName("totalReps")
                .OverridePropertyName("totalReps");

            RuleFor(er => er.TotalLifted)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total lifted cannot be negative")
                .WithName("totalLifted")
                .OverridePropertyName("totalLifted");
        }

        private void DateValidation()
        {
            // Max weight date cannot be in the future
            RuleFor(er => er.MaxWeightDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Max weight date cannot be in the future")
                .WithName("maxWeightDate")
                .OverridePropertyName("maxWeightDate");

            // Max reps date cannot be in the future
            RuleFor(er => er.MaxRepsDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Max reps date cannot be in the future")
                .WithName("maxRepsDate")
                .OverridePropertyName("maxRepsDate");

            // Max volume date cannot be in the future
            RuleFor(er => er.MaxVolumeDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Max volume date cannot be in the future")
                .WithName("maxVolumeDate")
                .OverridePropertyName("maxVolumeDate");

            // Max total volume date cannot be in the future
            RuleFor(er => er.MaxTotalVolumeDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Max total volume date cannot be in the future")
                .WithName("maxTotalVolumeDate")
                .OverridePropertyName("maxTotalVolumeDate");

            // Last performed cannot be in the future
            RuleFor(er => er.LastPerformed)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Last performed date cannot be in the future")
                .WithName("lastPerformed")
                .OverridePropertyName("lastPerformed");
        }
    }
}
