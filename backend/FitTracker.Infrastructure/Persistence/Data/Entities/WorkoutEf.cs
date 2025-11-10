using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
	/// Represents a workout session.
	/// </summary>
	public class WorkoutEf : BaseEntityEf
    {
        /// <summary>
        /// ID of the user performing the workout.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Optional ID of the template used to create this workout.
        /// </summary>
        public Guid? WorkoutTemplateId { get; set; }

        /// <summary>
        /// Workout name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Optional workout notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Date of the workout.
        /// </summary>
        public DateTime WorkoutDate { get; set; }

        /// <summary>
        /// Total duration of the workout.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Indicates if the workout is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Indicates if the workout is currently in progress.
        /// </summary>
        public bool IsInProgress { get; set; }

        /// <summary>
        /// Timestamp when the workout was started.
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// Timestamp when the workout was completed.
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Total volume lifted in kilograms.
        /// </summary>
        public decimal TotalVolumeKg { get; set; }

        /// <summary>
        /// Navigation to the user.
        /// </summary>
        public UserEf? User { get; set; }

        /// <summary>
        /// Navigation to the workout template (if used).
        /// </summary>
        public WorkoutTemplateEf? WorkoutTemplate { get; set; }

        /// <summary>
        /// Collection of exercises performed in this workout.
        /// </summary>
        public ICollection<WorkoutExerciseEf> Exercises { get; set; } = new HashSet<WorkoutExerciseEf>();
    }

}
