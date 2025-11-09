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

        public ICollection<WorkoutEf> Workouts { get; set; }
        public ICollection<ExerciseEf> CustomExercises { get; set; }
        public ICollection<WorkoutTemplateEf> WorkoutTemplates { get; set; }
        public ICollection<AchievementEf> Achievements { get; set; }
        public ICollection<ExerciseRecordEf> ExerciseRecords { get; set; }

        public UserEf()
        {
            Workouts = new HashSet<WorkoutEf>();
            CustomExercises = new HashSet<ExerciseEf>();
            WorkoutTemplates = new HashSet<WorkoutTemplateEf>();
            Achievements = new HashSet<AchievementEf>();
            ExerciseRecords = new HashSet<ExerciseRecordEf>();
        }
    }
}
