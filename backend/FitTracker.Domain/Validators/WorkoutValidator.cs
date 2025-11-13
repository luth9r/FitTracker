// FitTracker.Domain/Validators/WorkoutValidator.cs
using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
    internal class WorkoutValidator : AbstractValidator<Workout>
    {
        public WorkoutValidator()
        {
            Include(new BaseEntityValidator<Workout>());

            #region Required Fields

            RuleFor(w => w.UserId)
                .NotEmpty()
                .WithMessage("User ID is required")
                .WithName("userId")
                .OverridePropertyName("userId");

            RuleFor(w => w.Name)
                .NotEmpty()
                .WithMessage("Workout name is required")
                .WithName("name")
                .OverridePropertyName("name");

            RuleFor(w => w.WorkoutDate)
                .NotEmpty()
                .WithMessage("Workout date is required")
                .WithName("workoutDate")
                .OverridePropertyName("workoutDate");

            RuleFor(w => w.Duration)
                .NotNull()
                .WithMessage("Duration is required")
                .WithName("duration")
                .OverridePropertyName("duration");

            #endregion

            // Detailed validations
            NameValidation();
            NotesValidation();
            DurationValidation();
            VolumeValidation();
            DateValidation();
            StateValidation();
        }

        private void NameValidation()
        {
            RuleFor(w => w.Name)
                .Length(Workout.NameMinLength, Workout.NameMaxLength)
                .WithMessage($"Workout name must be between {Workout.NameMinLength} and {Workout.NameMaxLength} characters")
                .WithName("name")
                .OverridePropertyName("name");
        }

        private void NotesValidation()
        {
            RuleFor(w => w.Notes)
                .MaximumLength(Workout.NotesMaxLength)
                .When(w => !string.IsNullOrEmpty(w.Notes))
                .WithMessage($"Notes cannot exceed {Workout.NotesMaxLength} characters")
                .WithName("notes")
                .OverridePropertyName("notes");
        }

        private void DurationValidation()
        {
            RuleFor(w => w.Duration)
                .Must(d => d.TotalHours <= Workout.MaxDurationHours)
                .WithMessage($"Duration cannot exceed {Workout.MaxDurationHours} hours")
                .Must(d => d >= TimeSpan.Zero)
                .WithMessage("Duration cannot be negative")
                .WithName("duration")
                .OverridePropertyName("duration");
        }

        private void VolumeValidation()
        {
            RuleFor(w => w.TotalVolumeKg)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total volume cannot be negative")
                .WithName("totalVolumeKg")
                .OverridePropertyName("totalVolumeKg");
        }

        private void DateValidation()
        {
            // If completed, must have completion date
            RuleFor(w => w)
                .Must(w => !w.IsCompleted || w.CompletedAt.HasValue)
                .WithMessage("Completed workouts must have completion date")
                .WithName("completedAt")
                .OverridePropertyName("completedAt");

            // Completion date must be after or equal to workout date
            RuleFor(w => w)
                .Must(w => !w.CompletedAt.HasValue || w.CompletedAt.Value.Date >= w.WorkoutDate.Date)
                .WithMessage("Completion date cannot be before workout date")
                .WithName("completedAt")
                .OverridePropertyName("completedAt");

            // If in progress, must have started date
            RuleFor(w => w)
                .Must(w => !w.IsInProgress || w.StartedAt.HasValue)
                .WithMessage("In-progress workouts must have start date")
                .WithName("startedAt")
                .OverridePropertyName("startedAt");

            // Started date cannot be in the future
            RuleFor(w => w.StartedAt)
                .Must(d => !d.HasValue || d.Value <= DateTime.UtcNow)
                .WithMessage("Start date cannot be in the future")
                .WithName("startedAt")
                .OverridePropertyName("startedAt");
        }

        private void StateValidation()
        {
            // Cannot be both completed and in progress
            RuleFor(w => w)
                .Must(w => !(w.IsCompleted && w.IsInProgress))
                .WithMessage("Workout cannot be both completed and in progress")
                .WithName("isCompleted")
                .OverridePropertyName("isCompleted");

            // If in progress, duration should be > 0
            RuleFor(w => w)
                .Must(w => !w.IsInProgress || w.Duration > TimeSpan.Zero)
                .WithMessage("In-progress workout must have duration greater than zero")
                .WithName("duration")
                .OverridePropertyName("duration");

            // If completed but not started, invalid state
            RuleFor(w => w)
                .Must(w => !w.IsCompleted || w.StartedAt.HasValue)
                .WithMessage("Completed workout must have been started")
                .WithName("startedAt")
                .OverridePropertyName("startedAt");
        }
    }
}
