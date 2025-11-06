using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Domain.Entities
{
    public class Analytics
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string MuscleGroup { get; set; }

        public int TotalVolume { get; set; } // kg
        public decimal MaxWeight { get; set; }
        public int AvgReps { get; set; }
        public int TotalSets { get; set; }
        public int TotalReps { get; set; }

        public DateTime DateRecorded { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public User User { get; set; }
    }
}
