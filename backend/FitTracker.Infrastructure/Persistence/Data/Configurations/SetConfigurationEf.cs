using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class SetConfigurationEf : IEntityTypeConfiguration<SetEf>
    {
        public void Configure(EntityTypeBuilder<SetEf> builder)
        {
            builder.ToTable("sets");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(s => s.WorkoutExerciseId)
                .HasColumnName("workout_exercise_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(s => s.SetNumber)
                .HasColumnName("set_number")
                .IsRequired();

            builder.Property(s => s.WeightKg)
                .HasColumnName("weight_kg")
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(s => s.Reps)
                .HasColumnName("reps")
                .IsRequired();

            builder.Property(s => s.RestSeconds)
                .HasColumnName("rest_seconds");

            builder.Property(s => s.SetType)
                .HasColumnName("set_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(s => s.IsCompleted)
                .HasColumnName("is_completed")
                .IsRequired();

            builder.Property(s => s.CompletedAt)
                .HasColumnName("completed_at");

            builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(s => s.WorkoutExerciseId)
                .HasDatabaseName("IX_Sets_WorkoutExerciseId");

            builder.HasIndex(s => new { s.WorkoutExerciseId, s.SetNumber })
                .IsUnique()
                .HasDatabaseName("IX_Sets_WorkoutExercise_SetNumber");

            builder.HasIndex(s => s.IsCompleted)
                .HasDatabaseName("IX_Sets_IsCompleted");

            // Relationships
            builder.HasOne(s => s.WorkoutExercise)
                .WithMany(we => we.Sets)
                .HasForeignKey(s => s.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
