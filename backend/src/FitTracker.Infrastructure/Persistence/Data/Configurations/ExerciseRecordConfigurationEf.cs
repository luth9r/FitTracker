using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class ExerciseRecordConfigurationEf : IEntityTypeConfiguration<ExerciseRecordEf>
    {
        public void Configure(EntityTypeBuilder<ExerciseRecordEf> builder)
        {
            _ = builder.ToTable("exercise_records");

            _ = builder.HasKey(er => er.Id);

            _ = builder.Property(er => er.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            _ = builder.Property(er => er.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid")
                .IsRequired();

            _ = builder.Property(er => er.ExerciseId)
                .HasColumnName("exercise_id")
                .HasColumnType("uuid")
                .IsRequired();

            _ = builder.Property(er => er.MaxWeightKg)
                .HasColumnName("max_weight_kg")
                .HasPrecision(10, 2)
                .IsRequired();

            _ = builder.Property(er => er.MaxReps)
                .HasColumnName("max_reps")
                .IsRequired();

            _ = builder.Property(er => er.MaxVolumeKg)
                .HasColumnName("max_volume")
                .IsRequired();

            _ = builder.Property(er => er.MaxTotalVolumeKg)
                .HasColumnName("max_total_volume")
                .IsRequired();

            _ = builder.Property(er => er.MaxWeightDate)
                .HasColumnName("max_weight_date")
                .IsRequired();

            _ = builder.Property(er => er.MaxRepsDate)
                .HasColumnName("max_reps_date")
                .IsRequired();

            _ = builder.Property(er => er.MaxVolumeDate)
                .HasColumnName("max_volume_date")
                .IsRequired();

            _ = builder.Property(er => er.MaxTotalVolumeDate)
                .HasColumnName("max_total_volume_date")
                .IsRequired();

            _ = builder.Property(er => er.TotalWorkouts)
                .HasColumnName("total_workouts")
                .IsRequired();

            _ = builder.Property(er => er.TotalSets)
                .HasColumnName("total_sets")
                .IsRequired();

            _ = builder.Property(er => er.TotalReps)
                .HasColumnName("total_reps")
                .IsRequired();

            _ = builder.Property(er => er.TotalLiftedKg)
                .HasColumnName("total_lifted")
                .IsRequired();

            _ = builder.Property(er => er.LastPerformed)
                .HasColumnName("last_performed")
                .IsRequired();

            _ = builder.Property(er => er.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            _ = builder.Property(er => er.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            _ = builder.HasIndex(er => new { er.UserId, er.ExerciseId })
                .IsUnique()
                .HasDatabaseName("IX_ExerciseRecords_User_Exercise");

            _ = builder.HasIndex(er => er.LastPerformed)
                .HasDatabaseName("IX_ExerciseRecords_LastPerformed");

            // Relationships
            _ = builder.HasOne(er => er.User)
                .WithMany(u => u.ExerciseRecords)
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(er => er.Exercise)
                .WithMany()
                .HasForeignKey(er => er.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
