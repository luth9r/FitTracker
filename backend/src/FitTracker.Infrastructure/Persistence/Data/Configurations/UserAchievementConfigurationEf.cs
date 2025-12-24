using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class UserAchievementConfigurationEf : IEntityTypeConfiguration<UserAchievementEf>
    {
        public void Configure(EntityTypeBuilder<UserAchievementEf> builder)
        {
            _ = builder.ToTable("user_achievements");

            _ = builder.HasKey(ua => ua.Id);

            _ = builder.Property(ua => ua.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid")
                .IsRequired();

            _ = builder.Property(ua => ua.AchievementId)
                .HasColumnName("achievement_id")
                .HasColumnType("uuid")
                .IsRequired();

            _ = builder.Property(ua => ua.Progress)
                .HasColumnName("progress")
                .IsRequired()
                .HasDefaultValue(0);

            _ = builder.Property(ua => ua.IsUnlocked)
                .HasColumnName("is_unlocked")
                .IsRequired()
                .HasDefaultValue(false);

            _ = builder.Property(ua => ua.UnlockedAt)
                .HasColumnName("unlocked_at")
                .IsRequired(false);

            _ = builder.HasOne(ua => ua.UserEf)
                .WithMany(u => u.UserAchievements)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(ua => ua.AchievementEf)
                .WithMany()
                .HasForeignKey(ua => ua.AchievementId)
                .OnDelete(DeleteBehavior.Restrict);

            _ = builder.HasIndex(ua => new { ua.UserId, ua.AchievementId })
                .IsUnique()
                .HasDatabaseName("IX_user_achievements_user_id_achievement_id");

            _ = builder.HasIndex(ua => ua.UserId)
                .HasDatabaseName("IX_user_achievements_user_id");

            _ = builder.HasIndex(ua => ua.AchievementId)
                .HasDatabaseName("IX_user_achievements_achievement_id");
        }
    }
}
