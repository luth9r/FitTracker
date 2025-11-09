using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    public class UserEf : BaseEntityEf
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public string PreferredUnits { get; set; }

        public ICollection<WorkoutEf> Workouts { get; set; } = new HashSet<WorkoutEf>();
        public ICollection<ExerciseEf> CustomExercises { get; set; } = new HashSet<ExerciseEf>();
        public ICollection<WorkoutTemplateEf> WorkoutTemplates { get; set; } = new HashSet<WorkoutTemplateEf>();
        public ICollection<AchievementEf> Achievements { get; set; } = new HashSet<AchievementEf>();
        public ICollection<ExerciseRecordEf> ExerciseRecords { get; set; } = new HashSet<ExerciseRecordEf>();

    }
}
