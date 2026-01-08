using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations;

public class ExerciseConfigurationEf : IEntityTypeConfiguration<ExerciseEf>
{
    public void Configure(EntityTypeBuilder<ExerciseEf> builder)
    {
        _ = builder.ToTable("exercises");

        _ = builder.HasKey(e => e.Id);

        _ = builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        _ = builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        _ = builder.Property(e => e.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(500);

        _ = builder.Property(e => e.VideoUrl)
            .HasColumnName("video_url")
            .HasMaxLength(500);

        _ = builder.Property(e => e.MuscleGroup)
            .HasColumnName("muscle_group")
            .IsRequired();

        _ = builder.Property(e => e.Equipment)
            .HasColumnName("equipment")
            .IsRequired();

        _ = builder.Property(e => e.CreatedByUserId)
            .HasColumnName("created_by_user_id")
            .IsRequired(false);

        _ = builder.HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CustomExercises)
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_exercises_name");

        _ = builder.HasIndex(e => e.MuscleGroup)
            .HasDatabaseName("IX_exercises_muscle_group");

        _ = builder.HasIndex(e => e.CreatedByUserId)
            .HasDatabaseName("IX_exercises_created_by_user_id");

        _ = builder.HasIndex(e => e.CreatedByUserId)
            .HasFilter("created_by_user_id IS NULL")
            .HasDatabaseName("IX_exercises_standard");
    }
}