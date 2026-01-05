namespace FitTracker.Infrastructure.Persistence.Data.Entities;

public class UserAchievementEf : BaseEntityEf
{
    public Guid UserId { get; set; }

    public Guid AchievementId { get; set; }

    public int Progress { get; set; }

    public bool IsUnlocked { get; set; }

    public DateTime? UnlockedAt { get; set; }

    public UserEf UserEf { get; set; } = default!;

    public AchievementEf AchievementEf { get; set; } = default!;
}
