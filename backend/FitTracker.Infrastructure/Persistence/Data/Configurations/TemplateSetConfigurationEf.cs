using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class TemplateSetConfigurationEf : IEntityTypeConfiguration<TemplateSetEf>
    {
        public void Configure(EntityTypeBuilder<TemplateSetEf> builder)
        {
            _ = builder.ToTable("template_sets");

            _ = builder.HasKey(ts => ts.Id);

            _ = builder.Property(ts => ts.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            _ = builder.Property(ts => ts.WorkoutTemplateExerciseId)
                .HasColumnName("workout_template_exercise_id")
                .HasColumnType("uuid")
                .IsRequired();

            _ = builder.Property(ts => ts.SetNumber)
                .HasColumnName("set_number")
                .IsRequired();

            _ = builder.Property(ts => ts.PlannedWeightKg)
                .HasColumnName("planned_weight")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            _ = builder.Property(ts => ts.PlannedReps)
                .HasColumnName("planned_reps")
                .IsRequired();

            _ = builder.Property(ts => ts.RestSeconds)
                .HasColumnName("rest_seconds");

            _ = builder.Property(ts => ts.SetType)
                .HasColumnName("set_type")
                .HasConversion<int>()
                .IsRequired();

            _ = builder.Property(ts => ts.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            _ = builder.Property(ts => ts.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Relationships
            _ = builder.HasOne(ts => ts.WorkoutTemplateExercise)
                .WithMany(te => te.PlannedSets)
                .HasForeignKey(ts => ts.WorkoutTemplateExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            _ = builder.HasIndex(ts => ts.WorkoutTemplateExerciseId)
                .HasDatabaseName("IX_TemplateSets_TemplateExerciseId");

            _ = builder.HasIndex(ts => new { ts.WorkoutTemplateExerciseId, ts.SetNumber })
                .IsUnique()
                .HasDatabaseName("IX_TemplateSets_TemplateExercise_SetNumber");
        }
    }
}
