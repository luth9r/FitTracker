using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class WorkoutConfigurationEf : IEntityTypeConfiguration<WorkoutEf>
    {
        public void Configure(EntityTypeBuilder<WorkoutEf> builder)
        {
            _ = builder.ToTable("workouts");

            _ = builder.HasKey(w => w.Id);

            _ = builder.Property(w => w.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            _ = builder.Property(w => w.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid")
                .IsRequired();

            _ = builder.Property(w => w.WorkoutTemplateId)
                .HasColumnName("workout_template_id")
                .HasColumnType("uuid");

            _ = builder.Property(w => w.Name)
                .HasColumnName("name")
                .HasMaxLength(Workout.NameMaxLength)
                .IsRequired();

            _ = builder.Property(w => w.Notes)
                .HasColumnName("notes")
                .HasMaxLength(Workout.NotesMaxLength);

            _ = builder.Property(w => w.WorkoutDate)
                .HasColumnName("workout_date")
                .IsRequired();

            _ = builder.Property(w => w.Duration)
                .HasColumnName("duration")
                .IsRequired();

            _ = builder.Property(w => w.IsCompleted)
                .HasColumnName("is_completed")
                .IsRequired();

            _ = builder.Property(w => w.IsInProgress)
                .HasColumnName("is_in_progress")
                .IsRequired();

            _ = builder.Property(w => w.StartedAt)
                .HasColumnName("started_at");

            _ = builder.Property(w => w.CompletedAt)
                .HasColumnName("completed_at");

            _ = builder.Property(w => w.TotalVolumeKg)
                .HasColumnName("total_volume_kg")
                .HasPrecision(10, 2)
                .IsRequired();

            _ = builder.Property(w => w.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            _ = builder.Property(w => w.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Relationships
            _ = builder.HasOne(w => w.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(w => w.WorkoutTemplate)
                .WithMany()
                .HasForeignKey(w => w.WorkoutTemplateId)
                .OnDelete(DeleteBehavior.SetNull);

            _ = builder.HasMany(w => w.Exercises)
                .WithOne(we => we.Workout)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            _ = builder.HasIndex(w => w.UserId)
                .HasDatabaseName("IX_Workouts_UserId");

            _ = builder.HasIndex(w => w.WorkoutDate)
                .HasDatabaseName("IX_Workouts_WorkoutDate");

            _ = builder.HasIndex(w => new { w.UserId, w.WorkoutDate })
                .HasDatabaseName("IX_Workouts_User_Date");

            _ = builder.HasIndex(w => w.IsCompleted)
                .HasDatabaseName("IX_Workouts_IsCompleted");

            _ = builder.HasIndex(w => w.WorkoutTemplateId)
                .HasDatabaseName("IX_Workouts_TemplateId");
        }
    }
}
