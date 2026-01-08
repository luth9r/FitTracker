using FitTracker.Domain.Abstract;

namespace FitTracker.Domain.Entities;

/// <summary>
///     Represents an achievement earned or in progress by a user.
/// </summary>
public class UserAchievement : BaseEntity
{
    /// <summary>
    ///     Gets the unique identifier of the user who owns this achievement record.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    ///     Gets the unique identifier of the achievement.
    /// </summary>
    public Guid AchievementId { get; private set; }

    /// <summary>
    ///     Gets the current progress value towards unlocking the achievement.
    /// </summary>
    public int Progress { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the achievement has been unlocked.
    /// </summary>
    public bool IsUnlocked { get; private set; }

    /// <summary>
    ///     Gets the date and time when the achievement was unlocked, or null if not yet unlocked.
    /// </summary>
    public DateTime? UnlockedAt { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserAchievement" /> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="achievementId">The unique identifier of the achievement.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="progress">The current progress.</param>
    /// <param name="isUnlocked">Whether the achievement is unlocked.</param>
    /// <param name="unlockedAt">The date and time unlocked.</param>
    /// <param name="createdAt">The date and time of creation.</param>
    /// <param name="updatedAt">The date and time of the last update.</param>
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

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserAchievement" /> class.
    /// </summary>
    private UserAchievement()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserAchievement" /> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="achievementId">The unique identifier of the achievement.</param>
    private UserAchievement(
        Guid userId,
        Guid achievementId)
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

    /// <summary>
    ///     Creates a new <see cref="UserAchievement" /> record.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="achievementId">The unique identifier of the achievement.</param>
    /// <returns>A new instance of <see cref="UserAchievement" />.</returns>
    public static UserAchievement Create(Guid userId, Guid achievementId)
    {
        return new UserAchievement(userId, achievementId);
    }

    /// <summary>
    ///     Updates the progress of the achievement and unlocks it if the target is reached.
    /// </summary>
    /// <param name="newProgress">The new progress value.</param>
    /// <param name="target">The target value required to unlock the achievement.</param>
    /// <returns><c>true</c> if the achievement was just unlocked; otherwise, <c>false</c>.</returns>
    public bool UpdateProgress(int newProgress, int target)
    {
        if (newProgress < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newProgress), newProgress, "Progress cannot be negative.");
        }

        if (target <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(target), target, "Target must be greater than zero.");
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

    /// <summary>
    ///     Calculates the progress percentage towards unlocking the achievement.
    /// </summary>
    /// <param name="target">The target value required to unlock the achievement.</param>
    /// <returns>The progress percentage.</returns>
    public int GetProgressPercentage(int target)
    {
        return target > 0 ? Progress * 100 / target : 0;
    }

    /// <summary>
    ///     Marks the achievement as unlocked.
    /// </summary>
    private void Unlock()
    {
        IsUnlocked = true;
        UnlockedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}