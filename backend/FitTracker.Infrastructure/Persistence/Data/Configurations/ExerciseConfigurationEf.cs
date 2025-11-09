using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class ExerciseConfigurationEf : IEntityTypeConfiguration<ExerciseEf>
    {
        public void Configure(EntityTypeBuilder<ExerciseEf> builder)
        {
            builder.ToTable("exercises");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(Exercise.NameMaxLength)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(Exercise.DescriptionMaxLength);

            builder.Property(e => e.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(Exercise.ImageUrlMaxLength);

            builder.Property(e => e.VideoUrl)
                .HasColumnName("video_url")
                .HasMaxLength(Exercise.VideoUrlMaxLength);

            builder.Property(e => e.MuscleGroup)
                .HasColumnName("muscle_group")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.Equipment)
                .HasColumnName("equipment")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.IsCustom)
                .HasColumnName("is_custom")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_Exercises_Name");

            builder.HasIndex(e => e.MuscleGroup)
                .HasDatabaseName("IX_Exercises_MuscleGroup");

            builder.HasIndex(e => e.Equipment)
                .HasDatabaseName("IX_Exercises_Equipment");

            builder.HasIndex(e => new { e.UserId, e.IsCustom })
                .HasDatabaseName("IX_Exercises_User_Custom");

            // Relationships
            builder.HasOne(e => e.User)
                .WithMany(u => u.CustomExercises)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
