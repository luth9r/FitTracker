using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.ImageUrl)
                .HasDefaultValue("https://via.placeholder.com/300x300?text=Exercise");

            builder.Property(e => e.VideoUrl)
                .HasDefaultValue("https://via.placeholder.com/300x300?text=Exercise");

            builder.Property(e => e.MuscleGroup)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Equipment)
                .HasMaxLength(50);

            builder.Property(e => e.IsCustom)
                .HasDefaultValue(false);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(e => new { e.IsCustom, e.MuscleGroup })
                .HasDatabaseName("IX_Exercise_IsCustom_MuscleGroup");

            builder.HasIndex(e => e.UserId);

            // Relationships
            builder.HasOne(e => e.User)
                .WithMany(u => u.CustomExercises)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.WorkoutExercises)
                .WithOne(we => we.Exercise)
                .HasForeignKey(we => we.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
