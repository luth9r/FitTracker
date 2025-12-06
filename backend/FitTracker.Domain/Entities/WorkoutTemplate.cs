namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Template for creating workouts.
    /// </summary>
    public class WorkoutTemplate : BaseEntity
    {
        public const int NameMaxLength = 100;
        public const int NameMinLength = 3;
        public const int DescriptionMaxLength = 1000;

        public Guid UserId { get; private set; }

        public string Name { get; private set; } = default!;

        public string? Description { get; private set; }

        public int UsageCount { get; private set; }

        public DateTime? LastUsedAt { get; private set; }

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

        private WorkoutTemplate()
        {
        }

        private WorkoutTemplate(Guid userId, string name, string? description = null)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(name) || name.Length < NameMinLength || name.Length > NameMaxLength)
            {
                throw new ArgumentException($"Name must be {NameMinLength}-{NameMaxLength} characters", nameof(name));
            }

            if (description?.Length > DescriptionMaxLength)
            {
                throw new ArgumentException($"Description cannot exceed {DescriptionMaxLength} characters", nameof(description));
            }

            UserId = userId;
            Name = name;
            Description = description;
            UsageCount = 0;
        }

        public static WorkoutTemplate Create(Guid userId, string name, string? description = null)
        {
            return new WorkoutTemplate(userId, name, description);
        }

        public void Update(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < NameMinLength || name.Length > NameMaxLength)
            {
                throw new ArgumentException($"Name must be {NameMinLength}-{NameMaxLength} characters", nameof(name));
            }

            if (description?.Length > DescriptionMaxLength)
            {
                throw new ArgumentException($"Description cannot exceed {DescriptionMaxLength} characters", nameof(description));
            }

            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RecordUsage()
        {
            UsageCount++;
            LastUsedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsFrequentlyUsed() => UsageCount >= 5;

        public bool WasUsedToday() => LastUsedAt?.Date == DateTime.UtcNow.Date;

        public TimeSpan? DaysSinceLastUse() => LastUsedAt.HasValue ? DateTime.UtcNow.Date - LastUsedAt.Value.Date : null;
    }
}
