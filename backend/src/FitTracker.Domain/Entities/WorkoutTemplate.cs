namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Template for creating workouts.
    /// </summary>
    public class WorkoutTemplate : BaseEntity
    {
        /// <summary>
        /// The maximum length allowed for the template name.
        /// </summary>
        public const int NameMaxLength = 100;

        /// <summary>
        /// The minimum length required for the template name.
        /// </summary>
        public const int NameMinLength = 3;

        /// <summary>
        /// The maximum length allowed for the template description.
        /// </summary>
        public const int DescriptionMaxLength = 1000;

        /// <summary>
        /// Gets the unique identifier of the user who owns the template.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the name of the template.
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets the description of the template.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Gets the total number of times this template has been used.
        /// </summary>
        public int UsageCount { get; private set; }

        /// <summary>
        /// Gets the date and time when the template was last used.
        /// </summary>
        public DateTime? LastUsedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutTemplate"/> class.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="name">The name of the template.</param>
        /// <param name="description">The description of the template.</param>
        /// <param name="usageCount">The usage count.</param>
        /// <param name="lastUsedAt">The date and time of last usage.</param>
        /// <param name="createdAt">The date and time of creation.</param>
        /// <param name="updatedAt">The date and time of the last update.</param>
        internal WorkoutTemplate(
            Guid id,
            Guid userId,
            string name,
            string? description,
            int usageCount,
            DateTime? lastUsedAt,
            DateTime createdAt,
            DateTime updatedAt)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Description = description;
            UsageCount = usageCount;
            LastUsedAt = lastUsedAt;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutTemplate"/> class.
        /// </summary>
        private WorkoutTemplate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutTemplate"/> class.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="name">The name of the template.</param>
        /// <param name="description">The description of the template.</param>
        private WorkoutTemplate(
            Guid userId,
            string name,
            string? description = null)
            : base()
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(name) || name.Length < NameMinLength || name.Length > NameMaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(name), name?.Length ?? 0, $"Name length must be between {NameMinLength} and {NameMaxLength} characters.");
            }

            if (description?.Length > DescriptionMaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(description), description.Length, $"Description length must not exceed {DescriptionMaxLength} characters.");
            }

            UserId = userId;
            Name = name;
            Description = description;
            UsageCount = 0;
        }

        /// <summary>
        /// Creates a new <see cref="WorkoutTemplate"/>.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="name">The name of the template.</param>
        /// <param name="description">The description of the template.</param>
        /// <returns>A new instance of <see cref="WorkoutTemplate"/>.</returns>
        public static WorkoutTemplate Create(Guid userId, string name, string? description = null)
        {
            return new WorkoutTemplate(userId, name, description);
        }

        /// <summary>
        /// Updates the template details.
        /// </summary>
        /// <param name="name">The new name.</param>
        /// <param name="description">The new description.</param>
        public void Update(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < NameMinLength || name.Length > NameMaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(name), name?.Length ?? 0, $"Name length must be between {NameMinLength} and {NameMaxLength} characters.");
            }

            if (description?.Length > DescriptionMaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(description), description.Length, $"Description length must not exceed {DescriptionMaxLength} characters.");
            }

            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Records that the template has been used.
        /// </summary>
        public void RecordUsage()
        {
            UsageCount++;
            LastUsedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Determines whether the template is frequently used.
        /// </summary>
        /// <returns><c>true</c> if frequently used; otherwise, <c>false</c>.</returns>
        public bool IsFrequentlyUsed() => UsageCount >= 5;

        /// <summary>
        /// Determines whether the template was used today.
        /// </summary>
        /// <returns><c>true</c> if used today; otherwise, <c>false</c>.</returns>
        public bool WasUsedToday() => LastUsedAt?.Date == DateTime.UtcNow.Date;

        /// <summary>
        /// Calculates the number of days elapsed since the last use of the template.
        /// </summary>
        /// <returns>The number of days, or null if never used.</returns>
        public TimeSpan? DaysSinceLastUse() => LastUsedAt.HasValue ? DateTime.UtcNow.Date - LastUsedAt.Value.Date : null;
    }
}
