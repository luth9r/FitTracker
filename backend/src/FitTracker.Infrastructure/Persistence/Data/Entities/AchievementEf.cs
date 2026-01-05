namespace FitTracker.Infrastructure.Persistence.Data.Entities;

/// <summary>
///     Represents a user achievement or milestone.
/// </summary>
public class AchievementEf : BaseEntityEf
{
    /// <summary>
    ///     Gets or sets type/category of the achievement.
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    ///     Gets or sets achievement name.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Gets or sets achievement description.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    ///     Gets or sets uRL to the achievement icon/badge.
    /// </summary>
    public string IconUrl { get; set; } = null!;

    /// <summary>
    ///     Gets or sets target value required to unlock the achievement.
    /// </summary>
    public int Target { get; set; }

    /// <summary>
    ///     Gets or sets achievement tier/level (bronze, silver, gold, etc.).
    /// </summary>
    public int Tier { get; set; }
}
