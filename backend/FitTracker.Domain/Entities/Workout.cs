using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Domain.Entities
{
    public class Workout
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ProgramId { get; set; }

        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime WorkoutDate { get; set; }
        public TimeSpan Duration { get; set; }
        public int TotalVolume { get; set; } // kg

        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Program Program { get; set; }
        public ICollection<WorkoutExercise> Exercises { get; set; } = new List<WorkoutExercise>();
    }
}
