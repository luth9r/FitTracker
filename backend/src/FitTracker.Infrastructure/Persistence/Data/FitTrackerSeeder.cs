using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Infrastructure.Persistence.Data
{
    [ExcludeFromCodeCoverage]
    public class FitTrackerSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var benchPressId = Guid.Parse("22222222-2222-2222-2222-222222222221");
            var squatId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var deadliftId = Guid.Parse("22222222-2222-2222-2222-222222222223");
            var overheadPressId = Guid.Parse("22222222-2222-2222-2222-222222222224");
            var pullUpsId = Guid.Parse("22222222-2222-2222-2222-222222222225");
            var barbellRowId = Guid.Parse("22222222-2222-2222-2222-222222222226");

            var customCurlId = Guid.Parse("22222222-2222-2222-2222-222222222227");

            var pushTemplateId = Guid.Parse("33333333-3333-3333-3333-333333333331");
            var pullTemplateId = Guid.Parse("33333333-3333-3333-3333-333333333332");
            var legsTemplateId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            var workout1Id = Guid.Parse("44444444-4444-4444-4444-444444444441");
            var workout2Id = Guid.Parse("44444444-4444-4444-4444-444444444442");

            SeedUser(modelBuilder, userId);
            SeedExercises(modelBuilder, userId, benchPressId, squatId, deadliftId,
                         overheadPressId, pullUpsId, barbellRowId, customCurlId);
            SeedAchievements(modelBuilder);
            SeedUserAchievements(modelBuilder, userId);
            SeedWorkoutTemplates(modelBuilder, userId, pushTemplateId,
                               pullTemplateId, legsTemplateId);
            SeedTemplateExercises(modelBuilder, pushTemplateId, pullTemplateId,
                                 legsTemplateId, benchPressId, squatId, deadliftId,
                                 overheadPressId, pullUpsId, barbellRowId);
            SeedWorkouts(modelBuilder, userId, workout1Id, workout2Id,
                        pushTemplateId, pullTemplateId);
            SeedWorkoutExercises(modelBuilder, workout1Id, workout2Id,
                               benchPressId, overheadPressId, pullUpsId, barbellRowId);
            SeedExerciseRecords(modelBuilder, userId, benchPressId, squatId,
                               deadliftId, overheadPressId);
        }

        private static void SeedUser(ModelBuilder modelBuilder, Guid userId)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _ = modelBuilder.Entity<UserEf>().HasData(
                new
                {
                    Id = userId,
                    Username = "fitness_pro",
                    Email = "fitnesspro@example.com",
                    PasswordHash = "$2a$11$20pVjNtw/EknLSvVjig.z.7aFDUPZhUajZoFvr2lncrVgR2/kNJMW",
                    FirstName = "John",
                    LastName = "Athlete",
                    Avatar = "https://example.com/avatars/john.jpg",
                    Bio = "Passionate about fitness and strength training. 5 years experience.",
                    IsEmailVerified = true,
                    GoogleProviderId = (string?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                });
        }

        private static void SeedExercises(ModelBuilder modelBuilder, Guid userId,
            Guid benchPressId, Guid squatId, Guid deadliftId,
            Guid overheadPressId, Guid pullUpsId, Guid barbellRowId, Guid customCurlId)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _ = modelBuilder.Entity<ExerciseEf>().HasData(

                new
                {
                    Id = benchPressId,
                    Name = "Barbell Bench Press",
                    Description = "Compound chest exercise performed on a flat bench",
                    ImageUrl = (string?)null,
                    VideoUrl = (string?)null,
                    MuscleGroup = 0,
                    Equipment = 1,
                    CreatedByUserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = squatId,
                    Name = "Barbell Back Squat",
                    Description = "Fundamental lower body compound exercise",
                    ImageUrl = (string?)null,
                    VideoUrl = (string?)null,
                    MuscleGroup = 3,
                    Equipment = 1,
                    CreatedByUserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = deadliftId,
                    Name = "Conventional Deadlift",
                    Description = "Full body compound pulling exercise",
                    ImageUrl = (string?)null,
                    VideoUrl = (string?)null,
                    MuscleGroup = 1,
                    Equipment = 1,
                    CreatedByUserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = overheadPressId,
                    Name = "Overhead Press",
                    Description = "Standing barbell shoulder press",
                    ImageUrl = (string?)null,
                    VideoUrl = (string?)null,
                    MuscleGroup = 2,
                    Equipment = 1,
                    CreatedByUserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = pullUpsId,
                    Name = "Pull-Ups",
                    Description = "Bodyweight vertical pulling exercise",
                    ImageUrl = (string?)null,
                    VideoUrl = (string?)null,
                    MuscleGroup = 1,
                    Equipment = 5,
                    CreatedByUserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = barbellRowId,
                    Name = "Barbell Row",
                    Description = "Bent-over barbell rowing exercise",
                    ImageUrl = (string?)null,
                    VideoUrl = (string?)null,
                    MuscleGroup = 1,
                    Equipment = 1,
                    CreatedByUserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },

                new
                {
                    Id = customCurlId,
                    Name = "John's Special Curl",
                    Description = "Custom bicep curl variation",
                    ImageUrl = (string?)null,
                    VideoUrl = (string?)null,
                    MuscleGroup = 4,
                    Equipment = 2,
                    CreatedByUserId = userId,
                    CreatedAt = baseDate.AddDays(30),
                    UpdatedAt = baseDate.AddDays(30),
                });
        }

        private static void SeedAchievements(ModelBuilder modelBuilder)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _ = modelBuilder.Entity<AchievementEf>().HasData(
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Type = 0,
                    Name = "First Steps",
                    Description = "Complete your first workout",
                    IconUrl = "/icons/achievement_first.png",
                    Target = 1,
                    Tier = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                    Type = 1,
                    Name = "Consistency King",
                    Description = "Complete 7 consecutive days of workouts",
                    IconUrl = "/icons/achievement_streak.png",
                    Target = 7,
                    Tier = 1,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                    Type = 2,
                    Name = "Century Club",
                    Description = "Complete 100 total workouts",
                    IconUrl = "/icons/achievement_century.png",
                    Target = 100,
                    Tier = 2,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                    Type = 3,
                    Name = "Iron Warrior",
                    Description = "Lift a total of 50,000 kg",
                    IconUrl = "/icons/achievement_iron.png",
                    Target = 50000,
                    Tier = 2,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                    Type = 4,
                    Name = "Record Breaker",
                    Description = "Set 20 personal records",
                    IconUrl = "/icons/achievement_pr.png",
                    Target = 20,
                    Tier = 1,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                });
        }

        private static void SeedUserAchievements(ModelBuilder modelBuilder, Guid userId)
        {
            var now = new DateTime(2024, 11, 10, 0, 0, 0, DateTimeKind.Utc);

            _ = modelBuilder.Entity<UserAchievementEf>().HasData(
                new
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    UserId = userId,
                    AchievementId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Progress = 1,
                    IsUnlocked = true,
                    UnlockedAt = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                    UserId = userId,
                    AchievementId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                    Progress = 7,
                    IsUnlocked = true,
                    UnlockedAt = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                    UserId = userId,
                    AchievementId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                    Progress = 63,
                    IsUnlocked = false,
                    UnlockedAt = (DateTime?)null,
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                    UserId = userId,
                    AchievementId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                    Progress = 50000,
                    IsUnlocked = true,
                    UnlockedAt = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb4"),
                    UserId = userId,
                    AchievementId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                    Progress = 12,
                    IsUnlocked = false,
                    UnlockedAt = (DateTime?)null,
                    CreatedAt = now,
                    UpdatedAt = now,
                });
        }

        private static void SeedWorkoutTemplates(
            ModelBuilder modelBuilder,
            Guid userId, Guid pushTemplateId, Guid pullTemplateId, Guid legsTemplateId)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _ = modelBuilder.Entity<WorkoutTemplateEf>().HasData(
                new
                {
                    Id = pushTemplateId,
                    UserId = userId,
                    Name = "Push Day",
                    Description = "Chest, shoulders, and triceps workout",
                    UsageCount = 8,
                    LastUsedAt = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = pullTemplateId,
                    UserId = userId,
                    Name = "Pull Day",
                    Description = "Back and biceps workout",
                    UsageCount = 8,
                    LastUsedAt = new DateTime(2024, 11, 9, 10, 0, 0, DateTimeKind.Utc),
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = legsTemplateId,
                    UserId = userId,
                    Name = "Leg Day",
                    Description = "Lower body workout",
                    UsageCount = 7,
                    LastUsedAt = new DateTime(2024, 11, 7, 10, 0, 0, DateTimeKind.Utc),
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                });
        }

        private static void SeedTemplateExercises(
            ModelBuilder modelBuilder,
            Guid pushTemplateId, Guid pullTemplateId, Guid legsTemplateId,
            Guid benchPressId, Guid squatId, Guid deadliftId,
            Guid overheadPressId, Guid pullUpsId, Guid barbellRowId)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var pushExercise1 = Guid.Parse("55555555-5555-5555-5555-555555555551");
            var pushExercise2 = Guid.Parse("55555555-5555-5555-5555-555555555552");
            var pullExercise1 = Guid.Parse("55555555-5555-5555-5555-555555555553");
            var pullExercise2 = Guid.Parse("55555555-5555-5555-5555-555555555554");
            var legsExercise1 = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var legsExercise2 = Guid.Parse("55555555-5555-5555-5555-555555555556");

            _ = modelBuilder.Entity<WorkoutTemplateExerciseEf>().HasData(
                new
                {
                    Id = pushExercise1,
                    WorkoutTemplateId = pushTemplateId,
                    ExerciseId = benchPressId,
                    OrderIndex = 1,
                    Notes = "Focus on controlled descent",
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = pushExercise2,
                    WorkoutTemplateId = pushTemplateId,
                    ExerciseId = overheadPressId,
                    OrderIndex = 2,
                    Notes = (string?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = pullExercise1,
                    WorkoutTemplateId = pullTemplateId,
                    ExerciseId = pullUpsId,
                    OrderIndex = 1,
                    Notes = "Add weight if possible",
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = pullExercise2,
                    WorkoutTemplateId = pullTemplateId,
                    ExerciseId = barbellRowId,
                    OrderIndex = 2,
                    Notes = (string?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = legsExercise1,
                    WorkoutTemplateId = legsTemplateId,
                    ExerciseId = squatId,
                    OrderIndex = 1,
                    Notes = "Go deep",
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = legsExercise2,
                    WorkoutTemplateId = legsTemplateId,
                    ExerciseId = deadliftId,
                    OrderIndex = 2,
                    Notes = "Keep back neutral",
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                });

            SeedTemplateSets(modelBuilder, pushExercise1, pushExercise2,
                           pullExercise1, pullExercise2, legsExercise1, legsExercise2);
        }

        private static void SeedTemplateSets(
            ModelBuilder modelBuilder,
            Guid pushEx1, Guid pushEx2, Guid pullEx1, Guid pullEx2,
            Guid legsEx1, Guid legsEx2)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _ = modelBuilder.Entity<TemplateSetEf>().HasData(

                // Bench Press
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666601"),
                    WorkoutTemplateExerciseId = pushEx1,
                    SetNumber = 1,
                    PlannedWeightKg = 60.0,
                    PlannedReps = 10,
                    RestSeconds = (int?)120,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666602"),
                    WorkoutTemplateExerciseId = pushEx1,
                    SetNumber = 2,
                    PlannedWeightKg = 70.0,
                    PlannedReps = 8,
                    RestSeconds = (int?)120,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666603"),
                    WorkoutTemplateExerciseId = pushEx1,
                    SetNumber = 3,
                    PlannedWeightKg = 80.0,
                    PlannedReps = 5,
                    RestSeconds = (int?)180,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },

                // Overhead Press
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666604"),
                    WorkoutTemplateExerciseId = pushEx2,
                    SetNumber = 1,
                    PlannedWeightKg = 40.0,
                    PlannedReps = 10,
                    RestSeconds = (int?)90,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666605"),
                    WorkoutTemplateExerciseId = pushEx2,
                    SetNumber = 2,
                    PlannedWeightKg = 45.0,
                    PlannedReps = 8,
                    RestSeconds = (int?)90,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                },
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666606"),
                    WorkoutTemplateExerciseId = pushEx2,
                    SetNumber = 3,
                    PlannedWeightKg = 50.0,
                    PlannedReps = 6,
                    RestSeconds = (int?)120,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate,
                });
        }

        private static void SeedWorkouts(ModelBuilder modelBuilder, Guid userId,
            Guid workout1Id, Guid workout2Id, Guid pushTemplateId, Guid pullTemplateId)
        {
            _ = modelBuilder.Entity<WorkoutEf>().HasData(
                new
                {
                    Id = workout1Id,
                    UserId = userId,
                    WorkoutTemplateId = (Guid?)pushTemplateId,
                    Name = "Push Day - Nov 8",
                    Notes = "Great workout, felt strong",
                    WorkoutDate = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    Duration = new TimeSpan(1, 15, 30),
                    IsCompleted = true,
                    IsInProgress = false,
                    StartedAt = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    CompletedAt = new DateTime(2024, 11, 8, 11, 15, 30, DateTimeKind.Utc),
                    TotalVolumeKg = 1450.50,
                    CreatedAt = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 11, 15, 30, DateTimeKind.Utc),
                },
                new
                {
                    Id = workout2Id,
                    UserId = userId,
                    WorkoutTemplateId = (Guid?)pullTemplateId,
                    Name = "Pull Day - Nov 9",
                    Notes = (string?)null,
                    WorkoutDate = new DateTime(2024, 11, 9, 0, 0, 0, DateTimeKind.Utc),
                    Duration = new TimeSpan(1, 5, 0),
                    IsCompleted = true,
                    IsInProgress = false,
                    StartedAt = new DateTime(2024, 11, 9, 10, 0, 0, DateTimeKind.Utc),
                    CompletedAt = new DateTime(2024, 11, 9, 11, 5, 0, DateTimeKind.Utc),
                    TotalVolumeKg = 1280.00,
                    CreatedAt = new DateTime(2024, 11, 9, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 11, 5, 0, DateTimeKind.Utc),
                });
        }

        private static void SeedWorkoutExercises(
            ModelBuilder modelBuilder,
            Guid workout1Id, Guid workout2Id, Guid benchPressId,
            Guid overheadPressId, Guid pullUpsId, Guid barbellRowId)
        {
            var workoutEx1 = Guid.Parse("77777777-7777-7777-7777-777777777771");
            var workoutEx2 = Guid.Parse("77777777-7777-7777-7777-777777777772");
            var workoutEx3 = Guid.Parse("77777777-7777-7777-7777-777777777773");
            var workoutEx4 = Guid.Parse("77777777-7777-7777-7777-777777777774");

            _ = modelBuilder.Entity<WorkoutExerciseEf>().HasData(
                new
                {
                    Id = workoutEx1,
                    WorkoutId = workout1Id,
                    ExerciseId = benchPressId,
                    OrderIndex = 1,
                    Notes = "Good form today",
                    CreatedAt = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 30, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = workoutEx2,
                    WorkoutId = workout1Id,
                    ExerciseId = overheadPressId,
                    OrderIndex = 2,
                    Notes = (string?)null,
                    CreatedAt = new DateTime(2024, 11, 8, 10, 35, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 11, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = workoutEx3,
                    WorkoutId = workout2Id,
                    ExerciseId = pullUpsId,
                    OrderIndex = 1,
                    Notes = "Added 10kg weight",
                    CreatedAt = new DateTime(2024, 11, 9, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 25, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = workoutEx4,
                    WorkoutId = workout2Id,
                    ExerciseId = barbellRowId,
                    OrderIndex = 2,
                    Notes = (string?)null,
                    CreatedAt = new DateTime(2024, 11, 9, 10, 30, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 11, 0, 0, DateTimeKind.Utc),
                });

            SeedWorkoutSets(modelBuilder, workoutEx1, workoutEx2, workoutEx3, workoutEx4);
        }

        private static void SeedWorkoutSets(
            ModelBuilder modelBuilder,
            Guid workoutEx1, Guid workoutEx2, Guid workoutEx3, Guid workoutEx4)
        {
            _ = modelBuilder.Entity<SetEf>().HasData(

                // Bench Press sets
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888801"),
                    WorkoutExerciseId = workoutEx1,
                    SetNumber = 1,
                    WeightKg = 60.0,
                    Reps = 10,
                    RestSeconds = (int?)120,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 5, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 3, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 5, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888802"),
                    WorkoutExerciseId = workoutEx1,
                    SetNumber = 2,
                    WeightKg = 70.0,
                    Reps = 8,
                    RestSeconds = (int?)120,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 10, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 7, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 10, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888803"),
                    WorkoutExerciseId = workoutEx1,
                    SetNumber = 3,
                    WeightKg = 80.0,
                    Reps = 6,
                    RestSeconds = (int?)180,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 15, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 12, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 15, 0, DateTimeKind.Utc),
                },

                // Overhead Press sets
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888804"),
                    WorkoutExerciseId = workoutEx2,
                    SetNumber = 1,
                    WeightKg = 40.0,
                    Reps = 10,
                    RestSeconds = (int?)90,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 40, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 38, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 40, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888805"),
                    WorkoutExerciseId = workoutEx2,
                    SetNumber = 2,
                    WeightKg = 45.0,
                    Reps = 8,
                    RestSeconds = (int?)90,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 45, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 42, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 45, 0, DateTimeKind.Utc),
                },

                // Pull-ups sets
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888806"),
                    WorkoutExerciseId = workoutEx3,
                    SetNumber = 1,
                    WeightKg = 10.0,
                    Reps = 8,
                    RestSeconds = (int?)120,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 9, 10, 5, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 9, 10, 3, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 5, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888807"),
                    WorkoutExerciseId = workoutEx3,
                    SetNumber = 2,
                    WeightKg = 10.0,
                    Reps = 7,
                    RestSeconds = (int?)120,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 9, 10, 10, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 9, 10, 7, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 10, 0, DateTimeKind.Utc),
                },

                // Barbell Row sets
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888808"),
                    WorkoutExerciseId = workoutEx4,
                    SetNumber = 1,
                    WeightKg = 80.0,
                    Reps = 8,
                    RestSeconds = (int?)90,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 9, 10, 35, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 9, 10, 33, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 35, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888809"),
                    WorkoutExerciseId = workoutEx4,
                    SetNumber = 2,
                    WeightKg = 85.0,
                    Reps = 6,
                    RestSeconds = (int?)90,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 9, 10, 40, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 9, 10, 37, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 40, 0, DateTimeKind.Utc),
                });
        }

        private static void SeedExerciseRecords(
            ModelBuilder modelBuilder,
            Guid userId, Guid benchPressId, Guid squatId, Guid deadliftId, Guid overheadPressId)
        {
            var now = new DateTime(2024, 11, 10, 0, 0, 0, DateTimeKind.Utc);

            _ = modelBuilder.Entity<ExerciseRecordEf>().HasData(
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999991"),
                    UserId = userId,
                    ExerciseId = benchPressId,
                    MaxWeightKg = 100.0,
                    MaxReps = 12,
                    MaxVolumeKg = 960.0,
                    MaxTotalVolumeKg = 4500.0,
                    MaxWeightDate = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    MaxRepsDate = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc),
                    MaxVolumeDate = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    MaxTotalVolumeDate = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    TotalWorkouts = 24,
                    TotalSets = 72,
                    TotalReps = 576,
                    TotalLiftedKg = 42000.0,
                    LastPerformed = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999992"),
                    UserId = userId,
                    ExerciseId = squatId,
                    MaxWeightKg = 140.0,
                    MaxReps = 10,
                    MaxVolumeKg = 1200.0,
                    MaxTotalVolumeKg = 5200.0,
                    MaxWeightDate = new DateTime(2024, 10, 20, 0, 0, 0, DateTimeKind.Utc),
                    MaxRepsDate = new DateTime(2024, 9, 10, 0, 0, 0, DateTimeKind.Utc),
                    MaxVolumeDate = new DateTime(2024, 10, 20, 0, 0, 0, DateTimeKind.Utc),
                    MaxTotalVolumeDate = new DateTime(2024, 11, 7, 0, 0, 0, DateTimeKind.Utc),
                    TotalWorkouts = 21,
                    TotalSets = 63,
                    TotalReps = 504,
                    TotalLiftedKg = 58000.0,
                    LastPerformed = new DateTime(2024, 11, 7, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999993"),
                    UserId = userId,
                    ExerciseId = deadliftId,
                    MaxWeightKg = 180.0,
                    MaxReps = 8,
                    MaxVolumeKg = 1280.0,
                    MaxTotalVolumeKg = 4800.0,
                    MaxWeightDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                    MaxRepsDate = new DateTime(2024, 8, 15, 0, 0, 0, DateTimeKind.Utc),
                    MaxVolumeDate = new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc),
                    MaxTotalVolumeDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                    TotalWorkouts = 18,
                    TotalSets = 54,
                    TotalReps = 324,
                    TotalLiftedKg = 48000.0,
                    LastPerformed = new DateTime(2024, 11, 7, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999994"),
                    UserId = userId,
                    ExerciseId = overheadPressId,
                    MaxWeightKg = 65.0,
                    MaxReps = 10,
                    MaxVolumeKg = 520.0,
                    MaxTotalVolumeKg = 2100.0,
                    MaxWeightDate = new DateTime(2024, 10, 18, 0, 0, 0, DateTimeKind.Utc),
                    MaxRepsDate = new DateTime(2024, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                    MaxVolumeDate = new DateTime(2024, 10, 18, 0, 0, 0, DateTimeKind.Utc),
                    MaxTotalVolumeDate = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    TotalWorkouts = 20,
                    TotalSets = 60,
                    TotalReps = 480,
                    TotalLiftedKg = 28000.0,
                    LastPerformed = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now,
                });
        }
    }
}
