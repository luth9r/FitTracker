using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    public class WorkoutTemplateExerciseEf : BaseEntityEf
    {
        public Guid WorkoutTemplateId { get; set; }
        public Guid ExerciseId { get; set; }
        public int OrderIndex { get; set; }
        public string? Notes { get; set; }

        public WorkoutTemplateEf? WorkoutTemplate { get; set; }
        public ExerciseEf? Exercise { get; set; }
        public ICollection<TemplateSetEf> PlannedSets { get; set; } = new HashSet<TemplateSetEf>();

    }
}
