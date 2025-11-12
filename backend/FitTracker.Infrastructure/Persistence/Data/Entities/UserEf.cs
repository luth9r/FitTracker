using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
	/// Represents a user account in the system.
	/// </summary>
	public class UserEf : BaseEntityEf
    {
        /// <summary>
        /// Unique username.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// User email address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Hashed password for authentication.
        /// </summary>
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// Optional first name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Optional last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Optional avatar image URL or path.
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Optional user biography.
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// Preferred measurement units (metric/imperial).
        /// </summary>
        public string PreferredUnits { get; set; }

        /// <summary>
        /// Verification status
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Collection of user's workout sessions.
        /// </summary>
        public ICollection<WorkoutEf> Workouts { get; set; } = new HashSet<WorkoutEf>();

        /// <summary>
        /// Collection of user's custom exercises.
        /// </summary>
        public ICollection<ExerciseEf> CustomExercises { get; set; } = new HashSet<ExerciseEf>();

        /// <summary>
        /// Collection of user's workout templates.
        /// </summary>
        public ICollection<WorkoutTemplateEf> WorkoutTemplates { get; set; } = new HashSet<WorkoutTemplateEf>();

        /// <summary>
        /// Collection of user's earned achievements.
        /// </summary>
        public ICollection<AchievementEf> Achievements { get; set; } = new HashSet<AchievementEf>();

        /// <summary>
        /// Collection of user's exercise records.
        /// </summary>
        public ICollection<ExerciseRecordEf> ExerciseRecords { get; set; } = new HashSet<ExerciseRecordEf>();
    }
}
