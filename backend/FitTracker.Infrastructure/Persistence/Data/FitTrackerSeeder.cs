using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Data
{
    public class FitTrackerSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            // Exercise IDs
            var benchPressId = Guid.Parse("22222222-2222-2222-2222-222222222221");
            var squatId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var deadliftId = Guid.Parse("22222222-2222-2222-2222-222222222223");
            var overheadPressId = Guid.Parse("22222222-2222-2222-2222-222222222224");
            var pullUpsId = Guid.Parse("22222222-2222-2222-2222-222222222225");
            var barbellRowId = Guid.Parse("22222222-2222-2222-2222-222222222226");

            // Template IDs
            var pushTemplateId = Guid.Parse("33333333-3333-3333-3333-333333333331");
            var pullTemplateId = Guid.Parse("33333333-3333-3333-3333-333333333332");
            var legsTemplateId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Workout IDs
            var workout1Id = Guid.Parse("44444444-4444-4444-4444-444444444441");
            var workout2Id = Guid.Parse("44444444-4444-4444-4444-444444444442");

            SeedUser(modelBuilder, userId);
            SeedExercises(modelBuilder, benchPressId, squatId, deadliftId,
                         overheadPressId, pullUpsId, barbellRowId);
            SeedWorkoutTemplates(modelBuilder, userId, pushTemplateId,
                               pullTemplateId, legsTemplateId);
            SeedTemplateExercises(modelBuilder, pushTemplateId, pullTemplateId,
                                 legsTemplateId, benchPressId, squatId, deadliftId,
                                 overheadPressId, pullUpsId, barbellRowId);
            SeedWorkouts(modelBuilder, userId, workout1Id, workout2Id,
                        pushTemplateId, pullTemplateId);
            SeedWorkoutExercises(modelBuilder, workout1Id, workout2Id,
                               benchPressId, overheadPressId, pullUpsId, barbellRowId);
            SeedSets(modelBuilder);
            SeedExerciseRecords(modelBuilder, userId, benchPressId, squatId,
                               deadliftId, overheadPressId);
            SeedAchievements(modelBuilder, userId);
        }

        private static void SeedUser(ModelBuilder modelBuilder, Guid userId)
        {
            modelBuilder.Entity<UserEf>().HasData(
                new
                {
                    Id = userId,
                    Username = "fitness_pro",
                    Email = "fitnesspro@example.com",
                    PasswordHash = "$2a$11$X5wFuQE5cCcYKfZ1EE.IbeQQfFhVxR4rL8CxKgE8X9Y.wU3jZ9r4C", // "Password123!"
                    FirstName = "John",
                    LastName = "Athlete",
                    Avatar = "https://example.com/avatars/john.jpg",
                    Bio = "Passionate about fitness and strength training. 5 years experience.",
                    PreferredUnits = "metric",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }

        private static void SeedExercises(ModelBuilder modelBuilder,
            Guid benchPressId, Guid squatId, Guid deadliftId,
            Guid overheadPressId, Guid pullUpsId, Guid barbellRowId)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<ExerciseEf>().HasData(
                new
                {
                    Id = benchPressId,
                    Name = "Barbell Bench Press",
                    Description = "Compound chest exercise performed on a flat bench",
                    ImageUrl = "https://example.com/exercises/bench-press.jpg",
                    VideoUrl = "https://example.com/videos/bench-press.mp4",
                    MuscleGroup = 0, // Chest
                    Equipment = 1, // Barbell
                    IsCustom = false,
                    UserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = squatId,
                    Name = "Barbell Back Squat",
                    Description = "Fundamental lower body compound exercise",
                    ImageUrl = "https://example.com/exercises/squat.jpg",
                    VideoUrl = "https://example.com/videos/squat.mp4",
                    MuscleGroup = 3, // Legs
                    Equipment = 1, // Barbell
                    IsCustom = false,
                    UserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = deadliftId,
                    Name = "Conventional Deadlift",
                    Description = "Full body compound pulling exercise",
                    ImageUrl = "https://example.com/exercises/deadlift.jpg",
                    VideoUrl = "https://example.com/videos/deadlift.mp4",
                    MuscleGroup = 1, // Back
                    Equipment = 1, // Barbell
                    IsCustom = false,
                    UserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = overheadPressId,
                    Name = "Overhead Press",
                    Description = "Standing barbell shoulder press",
                    ImageUrl = "https://example.com/exercises/ohp.jpg",
                    VideoUrl = "https://example.com/videos/ohp.mp4",
                    MuscleGroup = 2, // Shoulders
                    Equipment = 1, // Barbell
                    IsCustom = false,
                    UserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = pullUpsId,
                    Name = "Pull-Ups",
                    Description = "Bodyweight vertical pulling exercise",
                    ImageUrl = "https://example.com/exercises/pullups.jpg",
                    VideoUrl = "https://example.com/videos/pullups.mp4",
                    MuscleGroup = 1, // Back
                    Equipment = 5, // Bodyweight
                    IsCustom = false,
                    UserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = barbellRowId,
                    Name = "Barbell Row",
                    Description = "Bent-over barbell rowing exercise",
                    ImageUrl = "https://example.com/exercises/row.jpg",
                    VideoUrl = "https://example.com/videos/row.mp4",
                    MuscleGroup = 1, // Back
                    Equipment = 1, // Barbell
                    IsCustom = false,
                    UserId = (Guid?)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                }
            );
        }

        private static void SeedWorkoutTemplates(ModelBuilder modelBuilder,
            Guid userId, Guid pushTemplateId, Guid pullTemplateId, Guid legsTemplateId)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<WorkoutTemplateEf>().HasData(
                new
                {
                    Id = pushTemplateId,
                    UserId = userId,
                    Name = "Push Day",
                    Description = "Chest, shoulders, and triceps workout",
                    UsageCount = 8,
                    LastUsedAt = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
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
                    UpdatedAt = baseDate
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
                    UpdatedAt = baseDate
                }
            );
        }

        private static void SeedTemplateExercises(ModelBuilder modelBuilder,
            Guid pushTemplateId, Guid pullTemplateId, Guid legsTemplateId,
            Guid benchPressId, Guid squatId, Guid deadliftId,
            Guid overheadPressId, Guid pullUpsId, Guid barbellRowId)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Push template exercises
            var pushExercise1 = Guid.Parse("55555555-5555-5555-5555-555555555551");
            var pushExercise2 = Guid.Parse("55555555-5555-5555-5555-555555555552");

            // Pull template exercises
            var pullExercise1 = Guid.Parse("55555555-5555-5555-5555-555555555553");
            var pullExercise2 = Guid.Parse("55555555-5555-5555-5555-555555555554");

            // Legs template exercises
            var legsExercise1 = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var legsExercise2 = Guid.Parse("55555555-5555-5555-5555-555555555556");

            modelBuilder.Entity<WorkoutTemplateExerciseEf>().HasData(
                // Push Day
                new
                {
                    Id = pushExercise1,
                    WorkoutTemplateId = pushTemplateId,
                    ExerciseId = benchPressId,
                    OrderIndex = 1,
                    Notes = "Focus on controlled descent",
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = pushExercise2,
                    WorkoutTemplateId = pushTemplateId,
                    ExerciseId = overheadPressId,
                    OrderIndex = 2,
                    Notes = (string)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                // Pull Day
                new
                {
                    Id = pullExercise1,
                    WorkoutTemplateId = pullTemplateId,
                    ExerciseId = pullUpsId,
                    OrderIndex = 1,
                    Notes = "Add weight if possible",
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = pullExercise2,
                    WorkoutTemplateId = pullTemplateId,
                    ExerciseId = barbellRowId,
                    OrderIndex = 2,
                    Notes = (string)null,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                // Leg Day
                new
                {
                    Id = legsExercise1,
                    WorkoutTemplateId = legsTemplateId,
                    ExerciseId = squatId,
                    OrderIndex = 1,
                    Notes = "Go deep",
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = legsExercise2,
                    WorkoutTemplateId = legsTemplateId,
                    ExerciseId = deadliftId,
                    OrderIndex = 2,
                    Notes = "Keep back neutral",
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                }
            );

            // Seed template sets for each exercise
            SeedTemplateSets(modelBuilder, pushExercise1, pushExercise2,
                           pullExercise1, pullExercise2, legsExercise1, legsExercise2);
        }

        private static void SeedTemplateSets(ModelBuilder modelBuilder,
            Guid pushEx1, Guid pushEx2, Guid pullEx1, Guid pullEx2,
            Guid legsEx1, Guid legsEx2)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Bench Press template sets
            modelBuilder.Entity<TemplateSetEf>().HasData(
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666601"),
                    WorkoutTemplateExerciseId = pushEx1,
                    SetNumber = 1,
                    PlannedWeight = 60.0m,
                    PlannedReps = 10,
                    RestSeconds = 120,
                    SetType = 0, // Normal
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666602"),
                    WorkoutTemplateExerciseId = pushEx1,
                    SetNumber = 2,
                    PlannedWeight = 70.0m,
                    PlannedReps = 8,
                    RestSeconds = 120,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666603"),
                    WorkoutTemplateExerciseId = pushEx1,
                    SetNumber = 3,
                    PlannedWeight = 80.0m,
                    PlannedReps = 5,
                    RestSeconds = 180,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                // Overhead Press template sets
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666604"),
                    WorkoutTemplateExerciseId = pushEx2,
                    SetNumber = 1,
                    PlannedWeight = 40.0m,
                    PlannedReps = 10,
                    RestSeconds = 90,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666605"),
                    WorkoutTemplateExerciseId = pushEx2,
                    SetNumber = 2,
                    PlannedWeight = 45.0m,
                    PlannedReps = 8,
                    RestSeconds = 90,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666606"),
                    WorkoutTemplateExerciseId = pushEx2,
                    SetNumber = 3,
                    PlannedWeight = 50.0m,
                    PlannedReps = 6,
                    RestSeconds = 120,
                    SetType = 0,
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                }
            );
        }

        private static void SeedWorkouts(ModelBuilder modelBuilder, Guid userId,
            Guid workout1Id, Guid workout2Id, Guid pushTemplateId, Guid pullTemplateId)
        {
            modelBuilder.Entity<WorkoutEf>().HasData(
                new
                {
                    Id = workout1Id,
                    UserId = userId,
                    WorkoutTemplateId = pushTemplateId,
                    Name = "Push Day - Nov 8",
                    Notes = "Great workout, felt strong",
                    WorkoutDate = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    Duration = new TimeSpan(1, 15, 30),
                    IsCompleted = true,
                    IsInProgress = false,
                    StartedAt = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    CompletedAt = new DateTime(2024, 11, 8, 11, 15, 30, DateTimeKind.Utc),
                    TotalVolumeKg = 1450.50m,
                    CreatedAt = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 11, 15, 30, DateTimeKind.Utc)
                },
                new
                {
                    Id = workout2Id,
                    UserId = userId,
                    WorkoutTemplateId = pullTemplateId,
                    Name = "Pull Day - Nov 9",
                    Notes = (string)null,
                    WorkoutDate = new DateTime(2024, 11, 9, 10, 0, 0, DateTimeKind.Utc),
                    Duration = new TimeSpan(1, 5, 0),
                    IsCompleted = true,
                    IsInProgress = false,
                    StartedAt = new DateTime(2024, 11, 9, 10, 0, 0, DateTimeKind.Utc),
                    CompletedAt = new DateTime(2024, 11, 9, 11, 5, 0, DateTimeKind.Utc),
                    TotalVolumeKg = 1280.00m,
                    CreatedAt = new DateTime(2024, 11, 9, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 11, 5, 0, DateTimeKind.Utc)
                }
            );
        }

        private static void SeedWorkoutExercises(ModelBuilder modelBuilder,
            Guid workout1Id, Guid workout2Id, Guid benchPressId,
            Guid overheadPressId, Guid pullUpsId, Guid barbellRowId)
        {
            var workoutEx1 = Guid.Parse("77777777-7777-7777-7777-777777777771");
            var workoutEx2 = Guid.Parse("77777777-7777-7777-7777-777777777772");
            var workoutEx3 = Guid.Parse("77777777-7777-7777-7777-777777777773");
            var workoutEx4 = Guid.Parse("77777777-7777-7777-7777-777777777774");

            modelBuilder.Entity<WorkoutExerciseEf>().HasData(
                // Workout 1 exercises
                new
                {
                    Id = workoutEx1,
                    WorkoutId = workout1Id,
                    ExerciseId = benchPressId,
                    OrderIndex = 1,
                    Notes = "Good form today",
                    CreatedAt = new DateTime(2024, 11, 8, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 30, 0, DateTimeKind.Utc)
                },
                new
                {
                    Id = workoutEx2,
                    WorkoutId = workout1Id,
                    ExerciseId = overheadPressId,
                    OrderIndex = 2,
                    Notes = (string)null,
                    CreatedAt = new DateTime(2024, 11, 8, 10, 35, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 11, 0, 0, DateTimeKind.Utc)
                },
                // Workout 2 exercises
                new
                {
                    Id = workoutEx3,
                    WorkoutId = workout2Id,
                    ExerciseId = pullUpsId,
                    OrderIndex = 1,
                    Notes = "Added 10kg weight",
                    CreatedAt = new DateTime(2024, 11, 9, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 25, 0, DateTimeKind.Utc)
                },
                new
                {
                    Id = workoutEx4,
                    WorkoutId = workout2Id,
                    ExerciseId = barbellRowId,
                    OrderIndex = 2,
                    Notes = (string)null,
                    CreatedAt = new DateTime(2024, 11, 9, 10, 30, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 11, 0, 0, DateTimeKind.Utc)
                }
            );

            SeedWorkoutSets(modelBuilder, workoutEx1, workoutEx2, workoutEx3, workoutEx4);
        }

        private static void SeedSets(ModelBuilder modelBuilder)
        {
            // Sets seeded in SeedWorkoutSets
        }

        private static void SeedWorkoutSets(ModelBuilder modelBuilder,
            Guid workoutEx1, Guid workoutEx2, Guid workoutEx3, Guid workoutEx4)
        {
            modelBuilder.Entity<SetEf>().HasData(
                // Bench Press sets (workout 1)
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888801"),
                    WorkoutExerciseId = workoutEx1,
                    SetNumber = 1,
                    WeightKg = 60.0m,
                    Reps = 10,
                    RestSeconds = 120,
                    SetType = 0, // Normal
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 5, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 3, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 5, 0, DateTimeKind.Utc)
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888802"),
                    WorkoutExerciseId = workoutEx1,
                    SetNumber = 2,
                    WeightKg = 70.0m,
                    Reps = 8,
                    RestSeconds = 120,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 10, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 7, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 10, 0, DateTimeKind.Utc)
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888803"),
                    WorkoutExerciseId = workoutEx1,
                    SetNumber = 3,
                    WeightKg = 80.0m,
                    Reps = 6,
                    RestSeconds = 180,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 15, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 12, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 15, 0, DateTimeKind.Utc)
                },
                // Overhead Press sets (workout 1)
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888804"),
                    WorkoutExerciseId = workoutEx2,
                    SetNumber = 1,
                    WeightKg = 40.0m,
                    Reps = 10,
                    RestSeconds = 90,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 40, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 38, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 40, 0, DateTimeKind.Utc)
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888805"),
                    WorkoutExerciseId = workoutEx2,
                    SetNumber = 2,
                    WeightKg = 45.0m,
                    Reps = 8,
                    RestSeconds = 90,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 8, 10, 45, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 8, 10, 42, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 8, 10, 45, 0, DateTimeKind.Utc)
                },
                // Pull-ups sets (workout 2)
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888806"),
                    WorkoutExerciseId = workoutEx3,
                    SetNumber = 1,
                    WeightKg = 10.0m, // Added weight
                    Reps = 8,
                    RestSeconds = 120,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 9, 10, 5, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 9, 10, 3, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 5, 0, DateTimeKind.Utc)
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888807"),
                    WorkoutExerciseId = workoutEx3,
                    SetNumber = 2,
                    WeightKg = 10.0m,
                    Reps = 7,
                    RestSeconds = 120,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 9, 10, 10, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 9, 10, 7, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 10, 0, DateTimeKind.Utc)
                },
                // Barbell Row sets (workout 2)
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888808"),
                    WorkoutExerciseId = workoutEx4,
                    SetNumber = 1,
                    WeightKg = 80.0m,
                    Reps = 8,
                    RestSeconds = 90,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 9, 10, 35, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 9, 10, 33, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 35, 0, DateTimeKind.Utc)
                },
                new
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888809"),
                    WorkoutExerciseId = workoutEx4,
                    SetNumber = 2,
                    WeightKg = 85.0m,
                    Reps = 6,
                    RestSeconds = 90,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = new DateTime(2024, 11, 9, 10, 40, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 11, 9, 10, 37, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 9, 10, 40, 0, DateTimeKind.Utc)
                }
            );
        }

        private static void SeedExerciseRecords(ModelBuilder modelBuilder,
            Guid userId, Guid benchPressId, Guid squatId, Guid deadliftId, Guid overheadPressId)
        {
            var now = new DateTime(2024, 11, 10, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<ExerciseRecordEf>().HasData(
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999991"),
                    UserId = userId,
                    ExerciseId = benchPressId,
                    MaxWeight_Kilograms = 100.0m,
                    MaxReps = 12,
                    MaxVolume = 960.0m, // 80kg * 12 Reps
                    MaxTotalVolume = 4500.0m,
                    MaxWeightDate = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    MaxRepsDate = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc),
                    MaxVolumeDate = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    MaxTotalVolumeDate = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    TotalWorkouts = 24,
                    TotalSets = 72,
                    TotalReps = 576,
                    TotalLifted = 42000.0m,
                    LastPerformed = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999992"),
                    UserId = userId,
                    ExerciseId = squatId,
                    MaxWeight_Kilograms = 140.0m,
                    MaxReps = 10,
                    MaxVolume = 1200.0m,
                    MaxTotalVolume = 5200.0m,
                    MaxWeightDate = new DateTime(2024, 10, 20, 0, 0, 0, DateTimeKind.Utc),
                    MaxRepsDate = new DateTime(2024, 9, 10, 0, 0, 0, DateTimeKind.Utc),
                    MaxVolumeDate = new DateTime(2024, 10, 20, 0, 0, 0, DateTimeKind.Utc),
                    MaxTotalVolumeDate = new DateTime(2024, 11, 7, 0, 0, 0, DateTimeKind.Utc),
                    TotalWorkouts = 21,
                    TotalSets = 63,
                    TotalReps = 504,
                    TotalLifted = 58000.0m,
                    LastPerformed = new DateTime(2024, 11, 7, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999993"),
                    UserId = userId,
                    ExerciseId = deadliftId,
                    MaxWeight_Kilograms = 180.0m,
                    MaxReps = 8,
                    MaxVolume = 1280.0m,
                    MaxTotalVolume = 4800.0m,
                    MaxWeightDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                    MaxRepsDate = new DateTime(2024, 8, 15, 0, 0, 0, DateTimeKind.Utc),
                    MaxVolumeDate = new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc),
                    MaxTotalVolumeDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                    TotalWorkouts = 18,
                    TotalSets = 54,
                    TotalReps = 324,
                    TotalLifted = 48000.0m,
                    LastPerformed = new DateTime(2024, 11, 7, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999994"),
                    UserId = userId,
                    ExerciseId = overheadPressId,
                    MaxWeight_Kilograms = 65.0m,
                    MaxReps = 10,
                    MaxVolume = 520.0m,
                    MaxTotalVolume = 2100.0m,
                    MaxWeightDate = new DateTime(2024, 10, 18, 0, 0, 0, DateTimeKind.Utc),
                    MaxRepsDate = new DateTime(2024, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                    MaxVolumeDate = new DateTime(2024, 10, 18, 0, 0, 0, DateTimeKind.Utc),
                    MaxTotalVolumeDate = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    TotalWorkouts = 20,
                    TotalSets = 60,
                    TotalReps = 480,
                    TotalLifted = 28000.0m,
                    LastPerformed = new DateTime(2024, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = now,
                    UpdatedAt = now
                }
            );
        }

        private static void SeedAchievements(ModelBuilder modelBuilder, Guid userId)
        {
            var now = new DateTime(2024, 11, 10, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<AchievementEf>().HasData(
                // First Workout - Unlocked
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    UserId = userId,
                    Type = 0, // First Workout
                    Name = "First Steps",
                    Description = "Complete your first workout",
                    IconUrl = "https://example.com/icons/first-workout.png",
                    Progress = 1,
                    Target = 1,
                    IsUnlocked = true,
                    UnlockedAt = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc),
                    Tier = 1,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                // Workout Streak - Unlocked
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                    UserId = userId,
                    Type = 1, // Workout Streak
                    Name = "Consistency King",
                    Description = "Complete 7 consecutive days of workouts",
                    IconUrl = "https://example.com/icons/streak.png",
                    Progress = 7,
                    Target = 7,
                    IsUnlocked = true,
                    UnlockedAt = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                    Tier = 2,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                // Total Workouts - In Progress
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                    UserId = userId,
                    Type = 2, // Total Workouts
                    Name = "Century Club",
                    Description = "Complete 100 total workouts",
                    IconUrl = "https://example.com/icons/century.png",
                    Progress = 63,
                    Target = 100,
                    IsUnlocked = false,
                    UnlockedAt = (DateTime?)null,
                    Tier = 3,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                // Weight Lifted - Unlocked
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                    UserId = userId,
                    Type = 3, // Weight Milestone
                    Name = "Iron Warrior",
                    Description = "Lift a total of 50,000 kg",
                    IconUrl = "https://example.com/icons/iron.png",
                    Progress = 50000,
                    Target = 50000,
                    IsUnlocked = true,
                    UnlockedAt = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    Tier = 3,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                // Personal Record - In Progress
                new
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                    UserId = userId,
                    Type = 4, // Personal Records
                    Name = "Record Breaker",
                    Description = "Set 20 personal records",
                    IconUrl = "https://example.com/icons/pr.png",
                    Progress = 12,
                    Target = 20,
                    IsUnlocked = false,
                    UnlockedAt = (DateTime?)null,
                    Tier = 2,
                    CreatedAt = now,
                    UpdatedAt = now
                }
            );
        }
    }
}

