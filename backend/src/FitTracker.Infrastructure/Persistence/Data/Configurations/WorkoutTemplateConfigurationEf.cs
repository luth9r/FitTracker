using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class WorkoutTemplateConfigurationEf : IEntityTypeConfiguration<WorkoutTemplateEf>
    {
        public void Configure(EntityTypeBuilder<WorkoutTemplateEf> builder)
        {
            _ = builder.ToTable("workout_templates");

            _ = builder.HasKey(wt => wt.Id);

            _ = builder.Property(wt => wt.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            _ = builder.Property(wt => wt.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid")
                .IsRequired();

            _ = builder.Property(wt => wt.Name)
                .HasColumnName("name")
                .HasMaxLength(WorkoutTemplate.NameMaxLength)
                .IsRequired();

            _ = builder.Property(wt => wt.Description)
                .HasColumnName("description")
                .HasMaxLength(WorkoutTemplate.DescriptionMaxLength);

            _ = builder.Property(wt => wt.UsageCount)
                .HasColumnName("usage_count")
                .IsRequired();

            _ = builder.Property(wt => wt.LastUsedAt)
                .HasColumnName("last_used_at");

            _ = builder.Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            _ = builder.Property(t => t.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Relationships
            _ = builder.HasOne(t => t.User)
                .WithMany(u => u.WorkoutTemplates)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(t => t.Exercises)
                .WithOne(e => e.WorkoutTemplate)
                .HasForeignKey(e => e.WorkoutTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            _ = builder.HasIndex(t => t.UserId)
                .HasDatabaseName("IX_WorkoutTemplates_UserId");

            _ = builder.HasIndex(t => new { t.UserId, t.LastUsedAt })
                .HasDatabaseName("IX_WorkoutTemplates_User_LastUsed");
        }
    }
}
