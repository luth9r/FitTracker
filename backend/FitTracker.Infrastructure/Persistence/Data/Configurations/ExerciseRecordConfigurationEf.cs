using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
	public class ExerciseRecordConfigurationEf : IEntityTypeConfiguration<ExerciseRecordEf>
	{
		public void Configure(EntityTypeBuilder<ExerciseRecordEf> builder)
		{
			builder.ToTable("exercise_records");

			builder.HasKey(er => er.Id);

			builder.Property(er => er.Id)
				.HasColumnName("id")
				.HasColumnType("uuid");

			builder.Property(er => er.UserId)
				.HasColumnName("user_id")
				.HasColumnType("uuid")
				.IsRequired();

			builder.Property(er => er.ExerciseId)
				.HasColumnName("exercise_id")
				.HasColumnType("uuid")
				.IsRequired();

			builder.Property(er => er.MaxWeight_Kilograms)
				.HasColumnName("max_weight_kg")
				.HasPrecision(10, 2)
				.IsRequired();

			builder.Property(er => er.MaxReps)
				.HasColumnName("max_reps")
				.IsRequired();

			builder.Property(er => er.MaxVolume)
				.HasColumnName("max_volume")
				.IsRequired();

			builder.Property(er => er.MaxTotalVolume)
				.HasColumnName("max_total_volume")
				.IsRequired();

			builder.Property(er => er.MaxWeightDate)
				.HasColumnName("max_weight_date")
				.IsRequired();

			builder.Property(er => er.MaxRepsDate)
				.HasColumnName("max_reps_date")
				.IsRequired();

			builder.Property(er => er.MaxVolumeDate)
				.HasColumnName("max_volume_date")
				.IsRequired();

			builder.Property(er => er.MaxTotalVolumeDate)
				.HasColumnName("max_total_volume_date")
				.IsRequired();

			builder.Property(er => er.TotalWorkouts)
				.HasColumnName("total_workouts")
				.IsRequired();

			builder.Property(er => er.TotalSets)
				.HasColumnName("total_sets")
				.IsRequired();

			builder.Property(er => er.TotalReps)
				.HasColumnName("total_reps")
				.IsRequired();

			builder.Property(er => er.TotalLifted)
				.HasColumnName("total_lifted")
				.IsRequired();

			builder.Property(er => er.LastPerformed)
				.HasColumnName("last_performed")
				.IsRequired();

			// Indexes
			builder.HasIndex(er => new { er.UserId, er.ExerciseId })
				.IsUnique()
				.HasDatabaseName("IX_ExerciseRecords_User_Exercise");

			builder.HasIndex(er => er.LastPerformed)
				.HasDatabaseName("IX_ExerciseRecords_LastPerformed");

			// Relationships
			builder.HasOne(er => er.User)
				.WithMany(u => u.ExerciseRecords)
				.HasForeignKey(er => er.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(er => er.Exercise)
				.WithMany()
				.HasForeignKey(er => er.ExerciseId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
