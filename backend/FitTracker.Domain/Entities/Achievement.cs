using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FluentValidation;

namespace FitTracker.Domain.Entities
{
    public class Achievement : BaseEntity
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;

        /// <summary>
        /// Gets the unique identifier of the user who owns this achievement.
        /// </summary>
        public Guid UserId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the type of the achievement.
        /// </summary>
        public AchievementType Type
        {
            get; private set;
        }

        /// <summary>
        /// Gets the name of the achievement.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the description of the achievement.
        /// </summary>
        public string Description
        {
            get; private set;
        }

        /// <summary>
        /// Gets the URL to the achievement's icon image.
        /// </summary>
        public string IconUrl
        {
            get; private set;
        }

        /// <summary>
        /// Gets the current progress value towards completing this achievement.
        /// </summary>
        public int Progress
        {
            get; private set;
        }

        /// <summary>
        /// Gets the target value required to unlock this achievement.
        /// </summary>
        public int Target
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether this achievement has been unlocked.
        /// </summary>
        public bool IsUnlocked
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date and time when this achievement was unlocked, or null if not yet unlocked.
        /// </summary>
        public DateTime? UnlockedAt
        {
            get; private set;
        }

        /// <summary>
        /// Gets the tier level of the achievement (Bronze, Silver, or Gold).
        /// </summary>
        public AchievementTier Tier
        {
            get; private set;
        }

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private Achievement()
        {
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new achievements.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        private Achievement(
            Guid userId,
            AchievementType type,
            string name,
            string description,
            int target,
            AchievementTier tier = AchievementTier.Bronze) : base()
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

            EnsureValid();
        }

        /// <summary>
        /// Constructor for restoring achievement from persistence layer.
        /// Use <see cref="AchievementBuilder"/> for creating new achievements.
        /// </summary>
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

            // No validation here since data is from persistence
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new AchievementValidator();
        }

        #endregion

        #region Domain Methods

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

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="AchievementBuilder"/> instance.
        /// </summary>
        public static AchievementBuilder CreateBuilder()
        {
            return new AchievementBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="Achievement"/> instances.
        /// </summary>
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

            /// <summary>
            /// Builds the <see cref="Achievement"/> entity.
            /// </summary>
            public Achievement Build()
            {
                return new Achievement(
                    _userId,
                    _type,
                    _name,
                    _description,
                    _target,
                    _tier);
            }
        }

        #endregion
    }
}
