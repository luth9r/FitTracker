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
            builder.ToTable("workouts");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(w => w.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(w => w.WorkoutTemplateId)
                .HasColumnName("workout_template_id")
                .HasColumnType("uuid");

            builder.Property(w => w.Name)
                .HasColumnName("name")
                .HasMaxLength(Workout.NameMaxLength)
                .IsRequired();

            builder.Property(w => w.Notes)
                .HasColumnName("notes")
                .HasMaxLength(Workout.NotesMaxLength);

            builder.Property(w => w.WorkoutDate)
                .HasColumnName("workout_date")
                .IsRequired();

            builder.Property(w => w.Duration)
                .HasColumnName("duration")
                .IsRequired();

            builder.Property(w => w.IsCompleted)
                .HasColumnName("is_completed")
                .IsRequired();

            builder.Property(w => w.IsInProgress)
                .HasColumnName("is_in_progress")
                .IsRequired();

            builder.Property(w => w.StartedAt)
                .HasColumnName("started_at");

            builder.Property(w => w.CompletedAt)
                .HasColumnName("completed_at");

            builder.Property(w => w.TotalVolumeKg)
                .HasColumnName("total_volume_kg")
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(w => w.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(w => w.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(w => w.UserId)
                .HasDatabaseName("IX_Workouts_UserId");

            builder.HasIndex(w => w.WorkoutDate)
                .HasDatabaseName("IX_Workouts_WorkoutDate");

            builder.HasIndex(w => new { w.UserId, w.WorkoutDate })
                .HasDatabaseName("IX_Workouts_User_Date");

            builder.HasIndex(w => w.IsCompleted)
                .HasDatabaseName("IX_Workouts_IsCompleted");

            builder.HasIndex(w => w.WorkoutTemplateId)
                .HasDatabaseName("IX_Workouts_TemplateId");

            // Relationships
            builder.HasOne(w => w.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.WorkoutTemplate)
                .WithMany()
                .HasForeignKey(w => w.WorkoutTemplateId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(w => w.Exercises)
                .WithOne(we => we.Workout)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
