using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class AchievementConfigurationEf : IEntityTypeConfiguration<AchievementEf>
    {
        public void Configure(EntityTypeBuilder<AchievementEf> builder)
        {
            builder.ToTable("achievements");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(a => a.Type)
                .HasColumnName("type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.Name)
                .HasColumnName("name")
                .HasMaxLength(Achievement.NameMaxLength)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .HasMaxLength(Achievement.DescriptionMaxLength)
                .IsRequired();

            builder.Property(a => a.IconUrl)
                .HasColumnName("icon_url")
                .IsRequired();

            builder.Property(a => a.Progress)
                .HasColumnName("progress")
                .IsRequired();

            builder.Property(a => a.Target)
                .HasColumnName("target")
                .IsRequired();

            builder.Property(a => a.IsUnlocked)
                .HasColumnName("is_unlocked")
                .IsRequired();

            builder.Property(a => a.UnlockedAt)
                .HasColumnName("unlocked_at");

            builder.Property(a => a.Tier)
                .HasColumnName("tier")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(a => a.UserId)
                .HasDatabaseName("IX_Achievements_UserId");

            builder.HasIndex(a => new { a.UserId, a.Type })
                .HasDatabaseName("IX_Achievements_User_Type");

            builder.HasIndex(a => a.IsUnlocked)
                .HasDatabaseName("IX_Achievements_IsUnlocked");

            // Relationships
            builder.HasOne(a => a.User)
                .WithMany(u => u.Achievements)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
