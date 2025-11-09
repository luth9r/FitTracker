using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    public class TemplateSetEf : BaseEntityEf
    {
        public Guid WorkoutTemplateExerciseId { get; set; }
        public int SetNumber { get; set; }
        public decimal PlannedWeight { get; set; }
        public int PlannedReps { get; set; }
        public int? RestSeconds { get; set; }
        public int SetType { get; set; }

        public WorkoutTemplateExerciseEf? WorkoutTemplateExercise { get; set; }
    }
}
