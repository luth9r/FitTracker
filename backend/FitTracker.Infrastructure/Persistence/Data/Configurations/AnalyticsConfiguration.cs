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
    public class AnalyticsConfiguration : IEntityTypeConfiguration<Analytics>
    {
        public void Configure(EntityTypeBuilder<Analytics> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.MuscleGroup)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.TotalVolume)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(a => a.MaxWeight)
                .HasPrecision(10, 2)
                .HasDefaultValue(0);

            builder.Property(a => a.AvgReps)
                .HasDefaultValue(0);

            builder.Property(a => a.TotalSets)
                .HasDefaultValue(0);

            builder.Property(a => a.TotalReps)
                .HasDefaultValue(0);

            builder.Property(a => a.DateRecorded)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(a => new { a.UserId, a.DateRecorded, a.MuscleGroup })
                .HasDatabaseName("IX_Analytics_User_Date_MuscleGroup");

            builder.HasIndex(a => new { a.UserId, a.MuscleGroup })
                .HasDatabaseName("IX_Analytics_User_MuscleGroup");

            builder.HasIndex(a => new { a.UserId, a.DateRecorded })
                .HasDatabaseName("IX_Analytics_User_Date");

            // Relationships
            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
