using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Domain.Entities
{
    public class WorkoutExercise
    {
        public Guid Id { get; set; }
        public Guid WorkoutId { get; set; }
        public Guid ExerciseId { get; set; }

        [Range(1, 1000)]
        public int OrderInWorkout { get; set; }

        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public Workout Workout { get; set; }
        public Exercise Exercise { get; set; }
        public ICollection<Set> Sets { get; set; } = new List<Set>();
    }
}
