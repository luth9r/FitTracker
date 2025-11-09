using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Configurations;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Data
{
    public class FitTrackerDbContext : DbContext
    {
        public FitTrackerDbContext(DbContextOptions<FitTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEf> Users { get; set; }
        public DbSet<WorkoutEf> Workouts { get; set; }
        public DbSet<WorkoutTemplateEf> WorkoutTemplates { get; set; }
        public DbSet<WorkoutExerciseEf> WorkoutExercises { get; set; }
        public DbSet<WorkoutTemplateExerciseEf> WorkoutTemplateExercises { get; set; }
        public DbSet<SetEf> Sets { get; set; }
        public DbSet<TemplateSetEf> TemplateSets { get; set; }
        public DbSet<ExerciseEf> Exercises { get; set; }
        public DbSet<AchievementEf> Achievements { get; set; }
        public DbSet<ExerciseRecordEf> ExerciseRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<UnitSystem>();
            modelBuilder.Ignore<Weight>();

            // Apply all entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitTrackerDbContext).Assembly);

            //SeedExercises(modelBuilder);
            //SeedTestUser(modelBuilder);
            //SeedWorkoutTemplates(modelBuilder);
            //SeedWorkouts(modelBuilder);
            //SeedExerciseRecords(modelBuilder);
            //SeedAchievements(modelBuilder);
        }

        #region Seed Exercises

        private void SeedExercises(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Chest exercises
            var chestExercises = new[]
            {
                CreateExercise("00000000-0000-0000-0000-000000000001", "Bench Press", "Classic barbell bench press", MuscleGroup.Chest, Equipment.Barbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000002", "Incline Bench Press", "Upper chest focus", MuscleGroup.Chest, Equipment.Barbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000003", "Dumbbell Flyes", "Chest isolation", MuscleGroup.Chest, Equipment.Dumbbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000004", "Push-ups", "Bodyweight chest exercise", MuscleGroup.Chest, Equipment.Bodyweight, seedDate),
            };

            // Back exercises
            var backExercises = new[]
            {
                CreateExercise("00000000-0000-0000-0000-000000000010", "Pull-ups", "Classic back exercise", MuscleGroup.Back, Equipment.Bodyweight, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000011", "Barbell Row", "Thick back builder", MuscleGroup.Back, Equipment.Barbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000012", "Lat Pulldown", "Lat width builder", MuscleGroup.Back, Equipment.Machine, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000013", "Deadlift", "Full back compound", MuscleGroup.Back, Equipment.Barbell, seedDate),
            };

            // Leg exercises
            var legExercises = new[]
            {
                CreateExercise("00000000-0000-0000-0000-000000000020", "Barbell Squat", "King of exercises", MuscleGroup.Legs, Equipment.Barbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000021", "Leg Press", "Quad focused machine", MuscleGroup.Legs, Equipment.Machine, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000022", "Romanian Deadlift", "Hamstring builder", MuscleGroup.Legs, Equipment.Barbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000023", "Leg Curl", "Hamstring isolation", MuscleGroup.Legs, Equipment.Machine, seedDate),
            };

            // Shoulder exercises
            var shoulderExercises = new[]
            {
                CreateExercise("00000000-0000-0000-0000-000000000030", "Overhead Press", "Shoulder compound", MuscleGroup.Shoulders, Equipment.Barbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000031", "Lateral Raises", "Side delt isolation", MuscleGroup.Shoulders, Equipment.Dumbbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000032", "Face Pulls", "Rear delt and health", MuscleGroup.Shoulders, Equipment.Cable, seedDate),
            };

            // Arm exercises
            var armExercises = new[]
            {
                CreateExercise("00000000-0000-0000-0000-000000000040", "Barbell Curl", "Bicep mass builder", MuscleGroup.Biceps, Equipment.Barbell, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000041", "Tricep Dips", "Tricep compound", MuscleGroup.Triceps, Equipment.Bodyweight, seedDate),
                CreateExercise("00000000-0000-0000-0000-000000000042", "Hammer Curls", "Brachialis focus", MuscleGroup.Biceps, Equipment.Barbell, seedDate),
            };

            modelBuilder.Entity<ExerciseEf>().HasData(
                chestExercises
                    .Concat(backExercises)
                    .Concat(legExercises)
                    .Concat(shoulderExercises)
                    .Concat(armExercises)
                    .ToArray()
            );
        }

        private object CreateExercise(string id, string name, string description, MuscleGroup muscleGroup, Equipment equipment, DateTime seedDate)
        {
            return new
            {
                Id = Guid.Parse(id),
                Name = name,
                Description = description,
                MuscleGroup = (int)muscleGroup,
                Equipment = (int)equipment,
                IsCustom = false,
                UserId = (Guid?)null,
                ImageUrl = (string?)null,
                VideoUrl = (string?)null,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            };
        }

        #endregion

        #region Seed Test User

        private void SeedTestUser(ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Password: "Test123!" hashed with BCrypt
            var passwordHash = "$2a$11$xvK5L4JZ8qvZ5hG3yH3k0.rR3tJwK8xYzV9nYZx3qT5wQ6hR7sP8m";

            modelBuilder.Entity<UserEf>().HasData(new
            {
                Id = userId,
                Username = "testuser",
                Email = "test@fittracker.com",
                PasswordHash = passwordHash,
                FirstName = "Test",
                LastName = "User",
                Bio = "Fitness enthusiast and gym lover 💪",
                Avatar = (string?)null,
                PreferredUnits = "Metric",
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            });
        }

        #endregion

        #region Seed Workout Templates

        private void SeedWorkoutTemplates(ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Template 1: Push Day
            var pushTemplateId = Guid.Parse("22222222-2222-2222-2222-222222222221");
            modelBuilder.Entity<WorkoutTemplateEf>().HasData(new
            {
                Id = pushTemplateId,
                UserId = userId,
                Name = "Push Day",
                Description = "Chest, shoulders, and triceps workout",
                IsPublic = false,
                UsageCount = 3,
                LastUsedAt = seedDate.AddDays(7),
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            });

            // Push template exercises
            var benchPressExerciseId = Guid.Parse("33333333-3333-3333-3333-333333333331");
            var inclineBenchExerciseId = Guid.Parse("33333333-3333-3333-3333-333333333332");
            var overheadPressExerciseId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<WorkoutTemplateExerciseEf>().HasData(
                new
                {
                    Id = benchPressExerciseId,
                    WorkoutTemplateId = pushTemplateId,
                    ExerciseId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Bench Press
                    OrderIndex = 1,
                    Notes = "Focus on form, 3 sec eccentric",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new
                {
                    Id = inclineBenchExerciseId,
                    WorkoutTemplateId = pushTemplateId,
                    ExerciseId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Incline Bench
                    OrderIndex = 2,
                    Notes = (string?)null,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new
                {
                    Id = overheadPressExerciseId,
                    WorkoutTemplateId = pushTemplateId,
                    ExerciseId = Guid.Parse("00000000-0000-0000-0000-000000000030"), // Overhead Press
                    OrderIndex = 3,
                    Notes = (string?)null,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );

            // Template sets for Bench Press
            modelBuilder.Entity<TemplateSetEf>().HasData(
                new
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444441"),
                    WorkoutTemplateExerciseId = benchPressExerciseId,
                    SetNumber = 1,
                    PlannedWeight = 60m,
                    PlannedReps = 12,
                    RestSeconds = (int?)120,
                    SetType = (int)SetType.WarmUp,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444442"),
                    WorkoutTemplateExerciseId = benchPressExerciseId,
                    SetNumber = 2,
                    PlannedWeight = 100m,
                    PlannedReps = 10,
                    RestSeconds = (int?)180,
                    SetType = (int)SetType.Normal,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444443"),
                    WorkoutTemplateExerciseId = benchPressExerciseId,
                    SetNumber = 3,
                    PlannedWeight = 100m,
                    PlannedReps = 8,
                    RestSeconds = (int?)180,
                    SetType = (int)SetType.Normal,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );

            // Template 2: Pull Day
            var pullTemplateId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            modelBuilder.Entity<WorkoutTemplateEf>().HasData(new
            {
                Id = pullTemplateId,
                UserId = userId,
                Name = "Pull Day",
                Description = "Back and biceps workout",
                UsageCount = 2,
                LastUsedAt = seedDate.AddDays(8),
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            });
        }

        #endregion

        #region Seed Workouts

        private void SeedWorkouts(ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Workout 1: Completed Push Day
            var workout1Id = Guid.Parse("55555555-5555-5555-5555-555555555551");
            modelBuilder.Entity<WorkoutEf>().HasData(new
            {
                Id = workout1Id,
                UserId = userId,
                WorkoutTemplateId = Guid.Parse("22222222-2222-2222-2222-222222222221"),
                Name = "Push Day",
                Notes = "Felt strong today!",
                WorkoutDate = seedDate.AddDays(7),
                Duration = TimeSpan.FromMinutes(75),
                IsCompleted = true,
                IsInProgress = false,
                StartedAt = seedDate.AddDays(7).AddHours(10),
                CompletedAt = seedDate.AddDays(7).AddHours(11).AddMinutes(15),
                TotalVolumeKg = 5000m,
                CreatedAt = seedDate.AddDays(7),
                UpdatedAt = seedDate.AddDays(7)
            });

            // Workout Exercise: Bench Press
            var workoutExercise1Id = Guid.Parse("66666666-6666-6666-6666-666666666661");
            modelBuilder.Entity<WorkoutExerciseEf>().HasData(new
            {
                Id = workoutExercise1Id,
                WorkoutId = workout1Id,
                ExerciseId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                OrderIndex = 1,
                Notes = (string?)null,
                CreatedAt = seedDate.AddDays(7),
                UpdatedAt = seedDate.AddDays(7)
            });

            // Sets for Bench Press
            modelBuilder.Entity<Set>().HasData(
                new
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777771"),
                    WorkoutExerciseId = workoutExercise1Id,
                    SetNumber = 1,
                    Weight_Kilograms = 60m,
                    Reps = 12,
                    RestSeconds = (int?)120,
                    IsWarmup = true,
                    SetType = SetType.WarmUp,
                    IsCompleted = true,
                    CompletedAt = seedDate.AddDays(7).AddHours(10).AddMinutes(5),
                    CreatedAt = seedDate.AddDays(7),
                    UpdatedAt = seedDate.AddDays(7)
                },
                new
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777772"),
                    WorkoutExerciseId = workoutExercise1Id,
                    SetNumber = 2,
                    Weight_Kilograms = 100m,
                    Reps = 10,
                    RestSeconds = (int?)180,
                    IsWarmup = false,
                    SetType = SetType.Normal,
                    IsCompleted = true,
                    CompletedAt = seedDate.AddDays(7).AddHours(10).AddMinutes(10),
                    CreatedAt = seedDate.AddDays(7),
                    UpdatedAt = seedDate.AddDays(7)
                },
                new
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777773"),
                    WorkoutExerciseId = workoutExercise1Id,
                    SetNumber = 3,
                    Weight_Kilograms = 100m,
                    Reps = 8,
                    RestSeconds = (int?)180,
                    IsWarmup = false,
                    SetType = SetType.Normal,
                    IsCompleted = true,
                    CompletedAt = seedDate.AddDays(7).AddHours(10).AddMinutes(15),
                    CreatedAt = seedDate.AddDays(7),
                    UpdatedAt = seedDate.AddDays(7)
                }
            );
        }

        #endregion

        #region Seed Exercise Records

        private void SeedExerciseRecords(ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Bench Press records
            modelBuilder.Entity<ExerciseRecordEf>().HasData(new
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888881"),
                UserId = userId,
                ExerciseId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                MaxWeight_Kilograms = 120m,
                MaxReps = 12,
                MaxVolume = 1200m,
                MaxTotalVolume = 3600m,
                MaxWeightDate = seedDate.AddDays(7),
                MaxRepsDate = seedDate.AddDays(5),
                MaxVolumeDate = seedDate.AddDays(7),
                MaxTotalVolumeDate = seedDate.AddDays(7),
                TotalWorkouts = 15,
                TotalSets = 45,
                TotalReps = 450,
                TotalLifted = 45000m,
                LastPerformed = seedDate.AddDays(7),
                CreatedAt = seedDate,
                UpdatedAt = seedDate.AddDays(7)
            });

            // Squat records
            modelBuilder.Entity<ExerciseRecordEf>().HasData(new
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888882"),
                UserId = userId,
                ExerciseId = Guid.Parse("00000000-0000-0000-0000-000000000020"),
                MaxWeight_Kilograms = 150m,
                MaxReps = 10,
                MaxVolume = 1500m,
                MaxTotalVolume = 4500m,
                MaxWeightDate = seedDate.AddDays(6),
                MaxRepsDate = seedDate.AddDays(4),
                MaxVolumeDate = seedDate.AddDays(6),
                MaxTotalVolumeDate = seedDate.AddDays(6),
                TotalWorkouts = 12,
                TotalSets = 36,
                TotalReps = 360,
                TotalLifted = 54000m,
                LastPerformed = seedDate.AddDays(6),
                CreatedAt = seedDate,
                UpdatedAt = seedDate.AddDays(6)
            });
        }

        #endregion

        #region Seed Achievements

        private void SeedAchievements(ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<AchievementEf>().HasData(
                // Unlocked achievements
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999991"),
                    UserId = userId,
                    Type = (int)AchievementType.FirstWorkout,
                    Name = "First Workout",
                    Description = "Complete your first workout",
                    IconUrl = "/icons/achievement_firstworkout.png",
                    Progress = 1,
                    Target = 1,
                    IsUnlocked = true,
                    UnlockedAt = (DateTime?)seedDate.AddDays(1),
                    Tier = (int)AchievementTier.Bronze,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate.AddDays(1)
                },
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999992"),
                    UserId = userId,
                    Type = (int)AchievementType.TotalWorkouts,
                    Name = "Workout Warrior",
                    Description = "Complete 10 workouts",
                    IconUrl = "/icons/achievement_totalworkouts.png",
                    Progress = 15,
                    Target = 10,
                    IsUnlocked = true,
                    UnlockedAt = (DateTime?)seedDate.AddDays(5),
                    Tier = (int)AchievementTier.Silver,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate.AddDays(7)
                },
                // In progress
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999993"),
                    UserId = userId,
                    Type = (int)AchievementType.WorkoutStreak,
                    Name = "Consistency King",
                    Description = "Complete 30 workouts in 30 days",
                    IconUrl = "/icons/achievement_workoutstreak.png",
                    Progress = 15,
                    Target = 30,
                    IsUnlocked = false,
                    UnlockedAt = (DateTime?)null,
                    Tier = (int)AchievementTier.Gold,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate.AddDays(7)
                },
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999994"),
                    UserId = userId,
                    Type = (int)AchievementType.WeightMilestone,
                    Name = "Century Club",
                    Description = "Bench press 100kg",
                    IconUrl = "/icons/achievement_weightmilestone.png",
                    Progress = 100,
                    Target = 100,
                    IsUnlocked = true,
                    UnlockedAt = (DateTime?)seedDate.AddDays(7),
                    Tier = (int)AchievementTier.Gold,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate.AddDays(7)
                }
            );
        }

        #endregion

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