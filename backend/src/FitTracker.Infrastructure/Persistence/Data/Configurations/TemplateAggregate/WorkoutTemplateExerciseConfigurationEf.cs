using FitTracker.Domain.Entities.TemplateAggregate;
using FitTracker.Infrastructure.Persistence.Data.Entities.TemplateAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations.TemplateAggregate;

public class WorkoutTemplateExerciseConfigurationEf : IEntityTypeConfiguration<TemplateWorkoutExerciseEf>
{
    public void Configure(EntityTypeBuilder<TemplateWorkoutExerciseEf> builder)
    {
        _ = builder.ToTable("workout_template_exercises");

        _ = builder.HasKey(wte => wte.Id);

        _ = builder.Property(wte => wte.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        _ = builder.Property(wte => wte.WorkoutTemplateId)
            .HasColumnName("workout_template_id")
            .HasColumnType("uuid")
            .IsRequired();

        _ = builder.Property(wte => wte.ExerciseId)
            .HasColumnName("exercise_id")
            .HasColumnType("uuid")
            .IsRequired();

        _ = builder.Property(wte => wte.OrderIndex)
            .HasColumnName("order_index")
            .IsRequired();

        _ = builder.Property(wte => wte.Notes)
            .HasColumnName("notes")
            .HasMaxLength(TemplateWorkoutExercise.NotesMaxLength);

        _ = builder.Property(wte => wte.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        _ = builder.Property(wte => wte.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        // Relationships
        _ = builder.HasOne(wte => wte.WorkoutTemplate)
            .WithMany(t => t.Exercises)
            .HasForeignKey(wte => wte.WorkoutTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = builder.HasOne(te => te.Exercise)
            .WithMany()
            .HasForeignKey(wte => wte.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        _ = builder.HasMany(wte => wte.PlannedSets)
            .WithOne(s => s.WorkoutTemplateExercise)
            .HasForeignKey(s => s.WorkoutTemplateExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        _ = builder.HasIndex(wte => wte.WorkoutTemplateId)
            .HasDatabaseName("IX_WorkoutTemplateExercises_TemplateId");

        _ = builder.HasIndex(wte => wte.ExerciseId)
            .HasDatabaseName("IX_WorkoutTemplateExercises_ExerciseId");

        _ = builder.HasIndex(wte => new { wte.WorkoutTemplateId, wte.OrderIndex })
            .IsUnique()
            .HasDatabaseName("IX_WorkoutTemplateExercises_Template_Order");
    }
}