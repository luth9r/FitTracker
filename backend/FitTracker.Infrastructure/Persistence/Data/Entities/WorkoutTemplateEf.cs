using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    public class WorkoutTemplateEf : BaseEntityEf
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int UsageCount { get; set; }
        public DateTime? LastUsedAt { get; set; }

        public UserEf? User { get; set; }
        public ICollection<WorkoutTemplateExerciseEf> Exercises { get; set; } = new HashSet<WorkoutTemplateExerciseEf>();
    }
}
