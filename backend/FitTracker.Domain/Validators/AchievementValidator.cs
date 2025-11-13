using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
    internal class AchievementValidator : AbstractValidator<Achievement>
    {
        public AchievementValidator()
        {
            Include(new BaseEntityValidator<Achievement>());

            #region Required Fields

            RuleFor(a => a.UserId)
                .NotEmpty()
                .WithMessage("User ID is required")
                .WithName("userId")
                .OverridePropertyName("userId");

            RuleFor(a => a.Type)
                .NotNull()
                .WithMessage("Achievement type is required")
                .WithName("type")
                .OverridePropertyName("type");

            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Achievement name is required")
                .WithName("name")
                .OverridePropertyName("name");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Achievement description is required")
                .WithName("description")
                .OverridePropertyName("description");

            RuleFor(a => a.Target)
                .NotEmpty()
                .WithMessage("Target is required")
                .WithName("target")
                .OverridePropertyName("target");

            RuleFor(a => a.Tier)
                .NotNull()
                .WithMessage("Achievement tier is required")
                .WithName("tier")
                .OverridePropertyName("tier");

            #endregion

            // Detailed validations
            NameValidation();
            DescriptionValidation();
            ProgressValidation();
            TypeValidation();
            TierValidation();
            UnlockValidation();
        }

        private void NameValidation()
        {
            RuleFor(a => a.Name)
                .Length(2, Achievement.NameMaxLength)
                .WithMessage($"Name must be between 2 and {Achievement.NameMaxLength} characters")
                .WithName("name")
                .OverridePropertyName("name");
        }

        private void DescriptionValidation()
        {
            RuleFor(a => a.Description)
                .Length(5, Achievement.DescriptionMaxLength)
                .WithMessage($"Description must be between 5 and {Achievement.DescriptionMaxLength} characters")
                .WithName("description")
                .OverridePropertyName("description");
        }

        private void ProgressValidation()
        {
            RuleFor(a => a.Progress)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Progress cannot be negative")
                .WithName("progress")
                .OverridePropertyName("progress");

            RuleFor(a => a.Target)
                .GreaterThan(0)
                .WithMessage("Target must be greater than 0")
                .WithName("target")
                .OverridePropertyName("target");

            // Progress cannot exceed target (unless unlocked)
            RuleFor(a => a)
                .Must(a => a.Progress <= a.Target || a.IsUnlocked)
                .WithMessage("Progress cannot exceed target unless achievement is unlocked")
                .WithName("progress")
                .OverridePropertyName("progress");
        }

        private void TypeValidation()
        {
            RuleFor(a => a.Type)
                .IsInEnum()
                .WithMessage("Invalid achievement type")
                .WithName("type")
                .OverridePropertyName("type");
        }

        private void TierValidation()
        {
            RuleFor(a => a.Tier)
                .IsInEnum()
                .WithMessage("Invalid achievement tier")
                .WithName("tier")
                .OverridePropertyName("tier");
        }

        private void UnlockValidation()
        {
            // If unlocked, must have unlock date
            RuleFor(a => a)
                .Must(a => !a.IsUnlocked || a.UnlockedAt.HasValue)
                .WithMessage("Unlocked achievements must have unlock date")
                .WithName("unlockedAt")
                .OverridePropertyName("unlockedAt");

            // Unlock date cannot be in the future
            RuleFor(a => a.UnlockedAt)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(a => a.UnlockedAt.HasValue)
                .WithMessage("Unlock date cannot be in the future")
                .WithName("unlockedAt")
                .OverridePropertyName("unlockedAt");

            // If unlocked, progress should equal or exceed target
            RuleFor(a => a)
                .Must(a => !a.IsUnlocked || a.Progress >= a.Target)
                .WithMessage("Unlocked achievement must have progress >= target")
                .WithName("progress")
                .OverridePropertyName("progress");
        }
    }
}
