using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class SetConfigurationEf : IEntityTypeConfiguration<SetEf>
    {
        public void Configure(EntityTypeBuilder<SetEf> builder)
        {
            _ = builder.ToTable("sets");

            _ = builder.HasKey(s => s.Id);

            _ = builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            _ = builder.Property(s => s.WorkoutExerciseId)
                .HasColumnName("workout_exercise_id")
                .HasColumnType("uuid")
                .IsRequired();

            _ = builder.Property(s => s.SetNumber)
                .HasColumnName("set_number")
                .IsRequired();

            _ = builder.Property(s => s.WeightKg)
                .HasColumnName("weight_kg")
                .HasPrecision(10, 2)
                .IsRequired();

            _ = builder.Property(s => s.Reps)
                .HasColumnName("reps")
                .IsRequired();

            _ = builder.Property(s => s.RestSeconds)
                .HasColumnName("rest_seconds");

            _ = builder.Property(s => s.SetType)
                .HasColumnName("set_type")
                .HasConversion<int>()
                .IsRequired();

            _ = builder.Property(s => s.IsCompleted)
                .HasColumnName("is_completed")
                .IsRequired();

            _ = builder.Property(s => s.CompletedAt)
                .HasColumnName("completed_at");

            _ = builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            _ = builder.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Relationships
            _ = builder.HasOne(s => s.WorkoutExercise)
                .WithMany(we => we.Sets)
                .HasForeignKey(s => s.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            _ = builder.HasIndex(s => s.WorkoutExerciseId)
                .HasDatabaseName("IX_Sets_WorkoutExerciseId");

            _ = builder.HasIndex(s => new { s.WorkoutExerciseId, s.SetNumber })
                .IsUnique()
                .HasDatabaseName("IX_Sets_WorkoutExercise_SetNumber");

            _ = builder.HasIndex(s => s.IsCompleted)
                .HasDatabaseName("IX_Sets_IsCompleted");
        }
    }
}
