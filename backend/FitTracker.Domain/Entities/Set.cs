using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Domain.Entities
{
    public class Set
    {
        public Guid Id { get; set; }
        public Guid WorkoutExerciseId { get; set; }

        [Range(1, 100)]
        public int SetNumber { get; set; }

        [Range(0, 1000)]
        public int Reps { get; set; }

        [Range(0, 10000)]
        public decimal Weight { get; set; } // kg or lbs

        [Range(1, 10)]
        public int? RPE { get; set; } // Rate of Perceived Exertion (1-10)

        public string SetType { get; set; } // Warm-up, Normal, Dropset, AMRAP, etc
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public WorkoutExercise WorkoutExercise { get; set; }
    }
}
