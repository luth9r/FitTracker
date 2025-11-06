using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Data
{
    public class FitTrackerDbContext : DbContext
    {
        public FitTrackerDbContext(DbContextOptions<FitTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<Analytics> Analytics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProgramConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutConfiguration());
            modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutExerciseConfiguration());
            modelBuilder.ApplyConfiguration(new SetConfiguration());
            modelBuilder.ApplyConfiguration(new UserFriendConfiguration());
            modelBuilder.ApplyConfiguration(new AnalyticsConfiguration());

            // Seed default exercises
            SeedDefaultExercises(modelBuilder);
        }

        private void SeedDefaultExercises(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var chestExercises = new[]
            {
                new Exercise
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "Bench press",
                    Description = "Basic exercise for chest",
                    MuscleGroup = "Chest",
                    Equipment = "Barbell",
                    IsCustom = false,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Exercise
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Name = "Dumbbell flyes",
                    Description = "Isolation exercise for chest",
                    MuscleGroup = "Chest",
                    Equipment = "Dumbbell",
                    IsCustom = false,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Exercise
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Name = "Push-ups",
                    Description = "Bodyweight exercise",
                    MuscleGroup = "Chest",
                    Equipment = "Bodyweight",
                    IsCustom = false,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            };

            var backExercises = new[]
            {
                new Exercise
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                    Name = "Pull-ups",
                    Description = "Basic back exercise",
                    MuscleGroup = "Back",
                    Equipment = "Bodyweight",
                    IsCustom = false,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            };

            var legExercises = new[]
            {
                new Exercise
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000020"),
                    Name = "Barbell squats",
                    Description = "Basic legs exercise",
                    MuscleGroup = "Legs",
                    Equipment = "Barbell",
                    IsCustom = false,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Exercise
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000021"),
                    Name = "Leg press",
                    Description = "Leg machine exercise",
                    MuscleGroup = "Legs",
                    Equipment = "Machine",
                    IsCustom = false,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            };

            modelBuilder.Entity<Exercise>().HasData(
                chestExercises.Concat(backExercises).Concat(legExercises).ToArray()
            );
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically set UpdatedAt for modified entities
            var modifiedEntries = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is not (User or Program or Workout or Exercise or UserFriend or Domain.Entities.Analytics))
                    continue;

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

