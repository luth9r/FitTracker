using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Domain.Entities
{
    [Table("Exercises")]
    public class Exercise
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; } = "https://via.placeholder.com/300x300?text=Exercise";

        public string? VideoUrl { get; set; } = "https://via.placeholder.com/300x300?text=Exercise";

        [Required]
        [StringLength(50)]
        public string MuscleGroup { get; set; } // Chest, Back, Legs, Shoulders, Arms, etc

        [StringLength(50)]
        public string Equipment { get; set; } // Barbell, Dumbbell, Machine, Bodyweight, etc

        // true = custom exercise, false = standard exercise
        public bool IsCustom { get; set; }

        // If custom exercise, UserId will be set
        public Guid? UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
