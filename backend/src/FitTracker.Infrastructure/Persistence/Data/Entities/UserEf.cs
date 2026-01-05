namespace FitTracker.Infrastructure.Persistence.Data.Entities;

/// <summary>
///     Represents a user account in the system.
/// </summary>
public class UserEf : BaseEntityEf
{
    /// <summary>
    ///     Gets or sets unique username.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    ///     Gets or sets user email address.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    ///     Gets or sets hashed password for authentication.
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    ///     Gets or sets optional first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    ///     Gets or sets optional last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    ///     Gets or sets optional avatar image URL or path.
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    ///     Gets or sets optional user biography.
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether verification status.
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier assigned to the user by the Google authentication provider.
    /// </summary>
    public string? GoogleProviderId { get; set; }

    /// <summary>
    ///     Gets or sets collection of user's workout sessions.
    /// </summary>
    public ICollection<WorkoutEf> Workouts { get; set; } = new HashSet<WorkoutEf>();

    /// <summary>
    ///     Gets or sets collection of user's custom exercises.
    /// </summary>
    public ICollection<ExerciseEf> CustomExercises { get; set; } = new HashSet<ExerciseEf>();

    /// <summary>
    ///     Gets or sets collection of user's workout templates.
    /// </summary>
    public ICollection<WorkoutTemplateEf> WorkoutTemplates { get; set; } = new HashSet<WorkoutTemplateEf>();

    /// <summary>
    ///     Gets or sets collection of user's earned achievements.
    /// </summary>
    public ICollection<UserAchievementEf> UserAchievements { get; set; } = new HashSet<UserAchievementEf>();

    /// <summary>
    ///     Gets or sets collection of user's exercise records.
    /// </summary>
    public ICollection<ExerciseRecordEf> ExerciseRecords { get; set; } = new HashSet<ExerciseRecordEf>();
}
