namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
	/// Represents a reusable workout template.
	/// </summary>
	public class WorkoutTemplateEf : BaseEntityEf
    {
        /// <summary>
        /// ID of the user who owns this template.
        /// </summary>
        public Guid UserId
        {
            get; set;
        }

        /// <summary>
        /// Template name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Optional template description.
        /// </summary>
        public string? Description
        {
            get; set;
        }

        /// <summary>
        /// Number of times this template has been used.
        /// </summary>
        public int UsageCount
        {
            get; set;
        }

        /// <summary>
        /// Timestamp of last template usage.
        /// </summary>
        public DateTime? LastUsedAt
        {
            get; set;
        }

        /// <summary>
        /// Navigation to the user.
        /// </summary>
        public UserEf? User
        {
            get; set;
        }

        /// <summary>
        /// Collection of exercises in this template.
        /// </summary>
        public ICollection<WorkoutTemplateExerciseEf> Exercises { get; set; } = new HashSet<WorkoutTemplateExerciseEf>();
    }
}
