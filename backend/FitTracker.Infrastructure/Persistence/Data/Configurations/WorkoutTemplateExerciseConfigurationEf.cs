using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class WorkoutTemplateExerciseConfigurationEf : IEntityTypeConfiguration<WorkoutTemplateExerciseEf>
    {
        public void Configure(EntityTypeBuilder<WorkoutTemplateExerciseEf> builder)
        {
            builder.ToTable("workout_template_exercises");

            builder.HasKey(wte => wte.Id);

            builder.Property(wte => wte.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(wte => wte.WorkoutTemplateId)
                .HasColumnName("workout_template_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(wte => wte.ExerciseId)
                .HasColumnName("exercise_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(wte => wte.OrderIndex)
                .HasColumnName("order_index")
                .IsRequired();

            builder.Property(wte => wte.Notes)
                .HasColumnName("notes")
                .HasMaxLength(WorkoutTemplateExercise.NotesMaxLength);

            builder.Property(wte => wte.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(wte => wte.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(wte => wte.WorkoutTemplateId)
                .HasDatabaseName("IX_WorkoutTemplateExercises_TemplateId");

            builder.HasIndex(wte => wte.ExerciseId)
                .HasDatabaseName("IX_WorkoutTemplateExercises_ExerciseId");

            builder.HasIndex(wte => new { wte.WorkoutTemplateId, wte.OrderIndex })
                .IsUnique()
                .HasDatabaseName("IX_WorkoutTemplateExercises_Template_Order");

            // Relationships
            builder.HasOne(wte => wte.WorkoutTemplate)
                .WithMany(t => t.Exercises)
                .HasForeignKey(wte => wte.WorkoutTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(te => te.Exercise)
                .WithMany()
                .HasForeignKey(wte => wte.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(wte => wte.PlannedSets)
                .WithOne(s => s.WorkoutTemplateExercise)
                .HasForeignKey(s => s.WorkoutTemplateExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
