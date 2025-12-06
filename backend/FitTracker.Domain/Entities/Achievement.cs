using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise achievement within the system.
    /// </summary>
    public class Achievement : BaseEntity
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;

        public AchievementType Type { get; private set; }

        public string Name { get; private set; } = default!;

        public string Description { get; private set; } = default!;

        public string IconUrl { get; private set; } = default!;

        public int Target { get; private set; }

        public AchievementTier Tier { get; private set; }

        internal Achievement(
            Guid id,
            AchievementType type,
            string name,
            string description,
            string iconUrl,
            int target,
            AchievementTier tier,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            Type = type;
            Name = name;
            Description = description;
            IconUrl = iconUrl;
            Target = target;
            Tier = tier;
        }

        private Achievement()
        {
        }

        public bool IsStreakAchievement() => Type == AchievementType.WorkoutStreak;

        public string GetProgressText(int progress) => $"{progress}/{Target}";

        public decimal ProgressPercentage(int progress) => (decimal)progress / Target * 100;
    }
}
