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
    public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Notes)
                .HasMaxLength(1000);

            builder.Property(w => w.WorkoutDate)
                .IsRequired();

            builder.Property(w => w.Duration)
                .IsRequired();

            builder.Property(w => w.TotalVolume)
                .HasDefaultValue(0);

            builder.Property(w => w.IsCompleted)
                .HasDefaultValue(false);

            builder.Property(w => w.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(w => w.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(w => new { w.UserId, w.WorkoutDate })
                .HasDatabaseName("IX_Workout_User_Date");

            builder.HasIndex(w => w.UserId);

            // Relationships
            builder.HasOne(w => w.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Program)
                .WithMany(p => p.Workouts)
                .HasForeignKey(w => w.ProgramId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(w => w.Exercises)
                .WithOne(we => we.Workout)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
