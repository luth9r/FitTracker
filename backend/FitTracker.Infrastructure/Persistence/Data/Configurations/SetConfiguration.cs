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
    public class SetConfiguration : IEntityTypeConfiguration<Set>
    {
        public void Configure(EntityTypeBuilder<Set> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SetNumber)
                .IsRequired();

            builder.Property(s => s.Reps)
                .IsRequired();

            builder.Property(s => s.Weight)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(s => s.SetType)
                .HasMaxLength(50)
                .HasDefaultValue("Normal");

            builder.Property(s => s.IsCompleted)
                .HasDefaultValue(false);

            builder.Property(s => s.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Relationships
            builder.HasOne(s => s.WorkoutExercise)
                .WithMany(we => we.Sets)
                .HasForeignKey(s => s.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
