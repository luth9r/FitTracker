namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
	/// Represents an exercise within a workout template.
	/// </summary>
	public class WorkoutTemplateExerciseEf : BaseEntityEf
    {
        /// <summary>
        /// ID of the workout template.
        /// </summary>
        public Guid WorkoutTemplateId
        {
            get; set;
        }

        /// <summary>
        /// ID of the exercise.
        /// </summary>
        public Guid ExerciseId
        {
            get; set;
        }

        /// <summary>
        /// Order of the exercise in the template.
        /// </summary>
        public int OrderIndex
        {
            get; set;
        }

        /// <summary>
        /// Optional notes for this exercise.
        /// </summary>
        public string? Notes
        {
            get; set;
        }

        /// <summary>
        /// Navigation to the workout template.
        /// </summary>
        public WorkoutTemplateEf? WorkoutTemplate
        {
            get; set;
        }

        /// <summary>
        /// Navigation to the exercise.
        /// </summary>
        public ExerciseEf? Exercise
        {
            get; set;
        }

        /// <summary>
        /// Collection of planned sets for this exercise.
        /// </summary>
        public ICollection<TemplateSetEf> PlannedSets { get; set; } = new HashSet<TemplateSetEf>();
    }
}
