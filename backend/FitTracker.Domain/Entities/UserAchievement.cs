namespace FitTracker.Domain.Entities
{
    public class UserAchievement : BaseEntity
    {
        public Guid UserId { get; private set; }

        public Guid AchievementId { get; private set; }

        public int Progress { get; private set; }

        public bool IsUnlocked { get; private set; }

        public DateTime? UnlockedAt { get; private set; }

        internal UserAchievement(
            Guid id,
            Guid achievementId,
            Guid userId,
            int progress,
            bool isUnlocked,
            DateTime? unlockedAt,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            AchievementId = achievementId;
            UserId = userId;
            Progress = progress;
            IsUnlocked = isUnlocked;
            UnlockedAt = unlockedAt;
        }

        private UserAchievement()
        {
        }

        private UserAchievement(Guid userId, Guid achievementId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty", nameof(userId));
            }

            if (achievementId == Guid.Empty)
            {
                throw new ArgumentException("AchievementId cannot be empty", nameof(achievementId));
            }

            UserId = userId;
            AchievementId = achievementId;
            Progress = 0;
            IsUnlocked = false;
        }

        public static UserAchievement Create(Guid userId, Guid achievementId)
        {
            return new UserAchievement(userId, achievementId);
        }

        public bool UpdateProgress(int newProgress, int target)
        {
            if (newProgress < 0)
            {
                throw new ArgumentException("Progress cannot be negative", nameof(newProgress));
            }

            Progress = newProgress;
            UpdatedAt = DateTime.UtcNow;

            if (!IsUnlocked && Progress >= target)
            {
                Unlock();
                return true;
            }

            return false;
        }

        public int GetProgressPercentage(int target)
        {
            return target > 0 ? (Progress * 100) / target : 0;
        }

        private void Unlock()
        {
            IsUnlocked = true;
            UnlockedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
