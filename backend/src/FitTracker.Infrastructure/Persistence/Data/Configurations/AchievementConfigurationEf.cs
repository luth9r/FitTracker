using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations;

public class AchievementConfigurationEf : IEntityTypeConfiguration<AchievementEf>
{
    public void Configure(EntityTypeBuilder<AchievementEf> builder)
    {
        _ = builder.ToTable("achievements");

        _ = builder.HasKey(a => a.Id);

        _ = builder.Property(a => a.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        _ = builder.Property(a => a.Type)
            .HasColumnName("type")
            .HasConversion<int>()
            .IsRequired();

        _ = builder.Property(a => a.Name)
            .HasColumnName("name")
            .HasMaxLength(Achievement.NameMaxLength)
            .IsRequired();

        _ = builder.Property(a => a.Description)
            .HasColumnName("description")
            .HasMaxLength(Achievement.DescriptionMaxLength)
            .IsRequired();

        _ = builder.Property(a => a.IconUrl)
            .HasColumnName("icon_url")
            .IsRequired();

        _ = builder.Property(a => a.Target)
            .HasColumnName("target")
            .IsRequired();

        _ = builder.Property(a => a.Tier)
            .HasColumnName("tier")
            .HasConversion<int>()
            .IsRequired();

        _ = builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        _ = builder.Property(a => a.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
    }
}
