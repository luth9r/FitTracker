using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class WorkoutExerciseConfigurationEf : IEntityTypeConfiguration<WorkoutExerciseEf>
    {
        public void Configure(EntityTypeBuilder<WorkoutExerciseEf> builder)
        {
            builder.ToTable("workout_exercises");

            builder.HasKey(we => we.Id);

            builder.Property(we => we.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(we => we.WorkoutId)
                .HasColumnName("workout_id")
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(we => we.ExerciseId)
                .HasColumnName("exercise_id")
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(we => we.OrderIndex)
                .HasColumnName("order_index")
                .IsRequired();

            builder.Property(we => we.Notes)
                .HasColumnName("notes")
                .HasMaxLength(WorkoutExercise.NotesMaxLength)
                .IsRequired(false);

            builder.Property(we => we.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(we => we.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(we => we.WorkoutId)
                .HasDatabaseName("IX_WorkoutExercises_WorkoutId");

            builder.HasIndex(we => we.ExerciseId)
                .HasDatabaseName("IX_WorkoutExercises_ExerciseId");

            builder.HasIndex(we => new { we.WorkoutId, we.OrderIndex })
                .IsUnique()
                .HasDatabaseName("IX_WorkoutExercises_Workout_Order");

            // Relationships
            builder.HasOne(we => we.Workout)
                .WithMany(w => w.Exercises)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(we => we.Sets)
                .WithOne(s => s.WorkoutExercise)
                .HasForeignKey(s => s.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
