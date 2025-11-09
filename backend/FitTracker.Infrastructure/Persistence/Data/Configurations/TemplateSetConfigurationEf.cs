using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
	public class TemplateSetConfigurationEf : IEntityTypeConfiguration<TemplateSetEf>
	{
		public void Configure(EntityTypeBuilder<TemplateSetEf> builder)
		{
			builder.ToTable("template_sets");

			builder.HasKey(ts => ts.Id);

			builder.Property(ts => ts.Id)
				.HasColumnName("id")
				.HasColumnType("uuid");

			builder.Property(ts => ts.WorkoutTemplateExerciseId)
				.HasColumnName("workout_template_exercise_id")
				.HasColumnType("uuid")
				.IsRequired();

			builder.Property(ts => ts.SetNumber)
				.HasColumnName("set_number")
				.IsRequired();

			builder.Property(ts => ts.PlannedWeight)
				.HasColumnName("planned_weight")
				.HasPrecision(10, 2)
				.IsRequired();

			builder.Property(ts => ts.PlannedReps)
				.HasColumnName("planned_reps")
				.IsRequired();

			builder.Property(ts => ts.RestSeconds)
				.HasColumnName("rest_seconds");

			builder.Property(ts => ts.SetType)
				.HasColumnName("set_type")
				.HasConversion<int>()
				.IsRequired();

			builder.Property(ts => ts.CreatedAt)
				.HasColumnName("created_at")
				.HasColumnType("timestamp with time zone")
				.HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

			builder.Property(ts => ts.UpdatedAt)
				.HasColumnName("updated_at")
				.HasColumnType("timestamp with time zone")
				.HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

			// Indexes
			builder.HasIndex(ts => ts.WorkoutTemplateExerciseId)
				.HasDatabaseName("IX_TemplateSets_TemplateExerciseId");

			builder.HasIndex(ts => new { ts.WorkoutTemplateExerciseId, ts.SetNumber })
				.IsUnique()
				.HasDatabaseName("IX_TemplateSets_TemplateExercise_SetNumber");

			// Relationships
			builder.HasOne(ts => ts.WorkoutTemplateExercise)
				.WithMany(te => te.PlannedSets)
				.HasForeignKey(ts => ts.WorkoutTemplateExerciseId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
