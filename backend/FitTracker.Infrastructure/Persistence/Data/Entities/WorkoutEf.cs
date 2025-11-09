using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    public class WorkoutEf : BaseEntityEf
    {
        public Guid UserId { get; set; }
        public Guid? WorkoutTemplateId { get; set; }
        public string Name { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime WorkoutDate { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsInProgress { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal TotalVolumeKg { get; set; }

        public UserEf? User { get; set; }
        public WorkoutTemplateEf? WorkoutTemplate { get; set; }
        public ICollection<WorkoutExerciseEf> Exercises { get; set; }

        public WorkoutEf()
        {
            Exercises = new HashSet<WorkoutExerciseEf>();
        }
    }

}
