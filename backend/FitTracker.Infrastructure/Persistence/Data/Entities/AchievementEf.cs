namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
    /// Represents a user achievement or milestone.
    /// </summary>
    public class AchievementEf : BaseEntityEf
    {
        /// <summary>
        /// ID of the user who earned this achievement.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Type/category of the achievement.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Achievement name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Achievement description.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// URL to the achievement icon/badge.
        /// </summary>
        public string IconUrl { get; set; } = null!;

        /// <summary>
        /// Current progress toward the achievement.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Target value required to unlock the achievement.
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// Indicates if the achievement has been unlocked.
        /// </summary>
        public bool IsUnlocked { get; set; }

        /// <summary>
        /// Timestamp when the achievement was unlocked.
        /// </summary>
        public DateTime? UnlockedAt { get; set; }

        /// <summary>
        /// Achievement tier/level (bronze, silver, gold, etc.).
        /// </summary>
        public int Tier { get; set; }

        /// <summary>
        /// Navigation to the user.
        /// </summary>
        public UserEf? User { get; set; }
    }
}
