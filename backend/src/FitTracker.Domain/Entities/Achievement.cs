using FitTracker.Domain.Abstract;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Entities;

/// <summary>
///     Represents an exercise achievement within the system.
/// </summary>
public class Achievement : BaseEntity
{
    /// <summary>
    ///     The maximum length allowed for the achievement name.
    /// </summary>
    public const int NameMaxLength = 100;

    /// <summary>
    ///     The maximum length allowed for the achievement description.
    /// </summary>
    public const int DescriptionMaxLength = 500;

    /// <summary>
    ///     Gets the type of the achievement.
    /// </summary>
    public AchievementType Type { get; }

    /// <summary>
    ///     Gets the name of the achievement.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    ///     Gets the description of the achievement.
    /// </summary>
    public string Description { get; private set; } = default!;

    /// <summary>
    ///     Gets the URL of the achievement icon.
    /// </summary>
    public string IconUrl { get; private set; } = default!;

    /// <summary>
    ///     Gets the target value required to unlock the achievement.
    /// </summary>
    public int Target { get; }

    /// <summary>
    ///     Gets the tier of the achievement.
    /// </summary>
    public AchievementTier Tier { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Achievement" /> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="type">The type of the achievement.</param>
    /// <param name="name">The name of the achievement.</param>
    /// <param name="description">The description of the achievement.</param>
    /// <param name="iconUrl">The URL of the achievement icon.</param>
    /// <param name="target">The target value required to unlock the achievement.</param>
    /// <param name="tier">The tier of the achievement.</param>
    /// <param name="createdAt">The date and time of creation.</param>
    /// <param name="updatedAt">The date and time of the last update.</param>
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

    /// <summary>
    ///     Initializes a new instance of the <see cref="Achievement" /> class.
    /// </summary>
    private Achievement()
    {
    }

    /// <summary>
    ///     Determines whether the achievement is related to a workout streak.
    /// </summary>
    /// <returns><c>true</c> if it is a streak achievement; otherwise, <c>false</c>.</returns>
    public bool IsStreakAchievement()
    {
        return Type == AchievementType.WorkoutStreak;
    }

    /// <summary>
    ///     Gets the progress text for the achievement.
    /// </summary>
    /// <param name="progress">The current progress value.</param>
    /// <returns>A string representing the progress.</returns>
    public string GetProgressText(int progress)
    {
        return $"{progress}/{Target}";
    }

    /// <summary>
    ///     Calculates the progress percentage for the achievement.
    /// </summary>
    /// <param name="progress">The current progress value.</param>
    /// <returns>The progress percentage.</returns>
    public decimal ProgressPercentage(int progress)
    {
        return (decimal)progress / Target * 100;
    }
}