using CSharpFunctionalExtensions;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise achievement within the system.
    /// </summary>
    public class Achievement : BaseEntity
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;

        public Guid UserId { get; private set; }
        public AchievementType Type { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string IconUrl { get; private set; }
        public int Progress { get; private set; }
        public int Target { get; private set; }
        public bool IsUnlocked { get; private set; }
        public DateTime? UnlockedAt { get; private set; }
        public AchievementTier Tier { get; private set; }

        private Achievement()
        {
            // For ORM
        }

        public Achievement(
            Guid userId,
            AchievementType type,
            string name,
            string description,
            int target,
            AchievementTier tier,
            int progress,
            bool isUnlocked,
            DateTime? unlockedAt) : base()
        {
            UserId = userId;
            Type = type;
            Name = name;
            Description = description;
            Target = target;
            Tier = tier;
            Progress = progress;
            IsUnlocked = isUnlocked;
            UnlockedAt = unlockedAt;
            IconUrl = $"/icons/achievement_{type.ToString().ToLower()}.png";
        }

        private Achievement(
        Guid userId,
        AchievementType type,
        string name,
        string description,
        int target,
        AchievementTier tier) : base()
        {
            UserId = userId;
            Type = type;
            Name = name;
            Description = description;
            Target = target;
            Tier = tier;
            Progress = 0;
            IsUnlocked = false;
            IconUrl = $"/icons/achievement_{type.ToString().ToLower()}.png";
        }

        protected override IValidator GetValidator()
        {
            return new AchievementValidator();
        }

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<Achievement>(this));
        }

        private Result<Achievement, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<Achievement, ValidationResult>(result);

            return Result.Success<Achievement, ValidationResult>(this);
        }

        public static Result<Achievement, ValidationResult> Create(
            Guid userId,
            AchievementType type,
            string name,
            string description,
            int target,
            AchievementTier tier = AchievementTier.Bronze)
        {
            var achievement = new Achievement(userId, type, name, description, target, tier);
            return achievement.ValidateWithResult();
        }

        public bool UpdateProgress(int newProgress)
        {
            Progress = newProgress;
            UpdatedAt = DateTime.UtcNow;

            if (!IsUnlocked && Progress >= Target)
            {
                Unlock();
                return true;
            }

            return false;
        }

        private void Unlock()
        {
            IsUnlocked = true;
            UnlockedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public int GetProgressPercentage()
        {
            return Target > 0 ? (Progress * 100) / Target : 0;
        }

        public Result<Achievement, ValidationResult> Update(
            string name,
            string description,
            int target,
            AchievementTier tier)
        {
            Name = name;
            Description = description;
            Target = target;
            Tier = tier;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public static AchievementBuilder CreateBuilder() => new AchievementBuilder();

        public class AchievementBuilder
        {
            private Guid _userId = Guid.NewGuid();
            private AchievementType _type = AchievementType.FirstWorkout;
            private string _name = "Default Achievement";
            private string _description = "Default description";
            private int _target = 100;
            private AchievementTier _tier = AchievementTier.Bronze;

            public AchievementBuilder WithUserId(Guid userId)
            {
                _userId = userId;
                return this;
            }

            public AchievementBuilder WithType(AchievementType type)
            {
                _type = type;
                return this;
            }

            public AchievementBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public AchievementBuilder WithDescription(string description)
            {
                _description = description;
                return this;
            }

            public AchievementBuilder WithTarget(int target)
            {
                _target = target;
                return this;
            }

            public AchievementBuilder WithTier(AchievementTier tier)
            {
                _tier = tier;
                return this;
            }

            public Result<Achievement, ValidationResult> Build()
            {
                var achievement = new Achievement(_userId, _type, _name, _description, _target, _tier);

                return achievement.ValidateWithResult();
            }
        }
    }
}
