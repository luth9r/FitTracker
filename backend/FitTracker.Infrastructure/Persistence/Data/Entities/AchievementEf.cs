using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Enums;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    public class AchievementEf : BaseEntityEf
    {
        public Guid UserId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public int Progress { get; set; }
        public int Target { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime? UnlockedAt { get; set; }
        public int Tier { get; set; }

        public UserEf? User { get; set; }
    }
}
