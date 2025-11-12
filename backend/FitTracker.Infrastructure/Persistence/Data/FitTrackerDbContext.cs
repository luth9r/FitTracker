using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Configurations;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Infrastructure.Persistence.Data
{
    [ExcludeFromCodeCoverage]
    public class FitTrackerDbContext : DbContext
    {
        public FitTrackerDbContext(DbContextOptions<FitTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEf> Users
        {
            get; set;
        }
        public DbSet<WorkoutEf> Workouts
        {
            get; set;
        }
        public DbSet<WorkoutTemplateEf> WorkoutTemplates
        {
            get; set;
        }
        public DbSet<WorkoutExerciseEf> WorkoutExercises
        {
            get; set;
        }
        public DbSet<WorkoutTemplateExerciseEf> WorkoutTemplateExercises
        {
            get; set;
        }
        public DbSet<SetEf> Sets
        {
            get; set;
        }
        public DbSet<TemplateSetEf> TemplateSets
        {
            get; set;
        }
        public DbSet<ExerciseEf> Exercises
        {
            get; set;
        }
        public DbSet<AchievementEf> Achievements
        {
            get; set;
        }
        public DbSet<ExerciseRecordEf> ExerciseRecords
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<UnitSystem>();
            modelBuilder.Ignore<Weight>();

            // Apply all entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitTrackerDbContext).Assembly);

            FitTrackerSeeder.SeedData(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically set UpdatedAt for modified entities
            var modifiedEntries = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");
                if (property != null)
                {
                    property.CurrentValue = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
