using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    public class WorkoutExerciseEf : BaseEntityEf
    {
        public Guid WorkoutId { get; set; }
        public Guid ExerciseId { get; set; }
        public int OrderIndex { get; set; }
        public string? Notes { get; set; }

        public WorkoutEf? Workout { get; set; }
        public ExerciseEf? Exercise { get; set; }
        public ICollection<SetEf> Sets { get; set; } = new HashSet<SetEf>();

    }
}
