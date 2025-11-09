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
    /// <summary>
    /// User achievements/badges
    /// </summary>
    public class Achievement : BaseEntity
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;

        public Guid UserId
        {
            get; private set;
        }
        public AchievementType Type
        {
            get; private set;
        }
        public string Name
        {
            get; private set;
        }
        public string Description
        {
            get; private set;
        }
        public string IconUrl
        {
            get; private set;
        }
        public int Progress
        {
            get; private set;
        }
        public int Target
        {
            get; private set;
        }
        public bool IsUnlocked
        {
            get; private set;
        }
        public DateTime? UnlockedAt
        {
            get; private set;
        }
        public AchievementTier Tier
        {
            get; private set;
        }   // Bronze, Silver, Gold

        private Achievement() : base()
        {
            Name = string.Empty;
            Description = string.Empty;
            IconUrl = string.Empty;
        }

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

            EnsureValid();
        }

        protected override IValidator GetValidator()
        {
            return new AchievementValidator();
        }

        public static Achievement Create(
            Guid userId,
            AchievementType type,
            string name,
            string description,
            int target,
            AchievementTier tier = AchievementTier.Bronze)
        {
            return new Achievement(userId, type, name, description, target, tier);
        }

        public bool UpdateProgress(int newProgress)
        {
            Progress = newProgress;
            UpdatedAt = DateTime.UtcNow;

            // Check if unlocked
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
    }
}
