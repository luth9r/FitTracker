using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Tests.Factories
{
    /// <summary>
    /// Factory for creating Achievement test data.
    /// </summary>
    public static class AchievementFactory
    {
        /// <summary>
        /// Creates a default achievement with minimal data.
        /// </summary>
        public static Achievement Default() => Achievement.CreateBuilder()
            .WithType(AchievementType.FirstWorkout)
            .WithName("Test Achievement")
            .WithDescription("Test description")
            .WithTarget(100)
            .WithTier(AchievementTier.Bronze)
            .Build();

        #region Workout Achievements

        /// <summary>
        /// Creates a "First Workout" achievement.
        /// </summary>
        public static Achievement FirstWorkout() => Achievement.CreateBuilder()
            .WithType(AchievementType.FirstWorkout)
            .WithName("First Steps")
            .WithDescription("Complete your first workout")
            .WithTarget(1)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates a "Workout Streak" achievement.
        /// </summary>
        public static Achievement WorkoutStreakBronze() => Achievement.CreateBuilder()
            .WithType(AchievementType.WorkoutStreak)
            .WithName("3-Day Streak")
            .WithDescription("Complete workouts for 3 days in a row")
            .WithTarget(3)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates a "Workout Streak" silver achievement.
        /// </summary>
        public static Achievement WorkoutStreakSilver() => Achievement.CreateBuilder()
            .WithType(AchievementType.WorkoutStreak)
            .WithName("7-Day Streak")
            .WithDescription("Complete workouts for 7 days in a row")
            .WithTarget(7)
            .WithTier(AchievementTier.Silver)
            .Build();

        /// <summary>
        /// Creates a "Workout Streak" gold achievement.
        /// </summary>
        public static Achievement WorkoutStreakGold() => Achievement.CreateBuilder()
            .WithType(AchievementType.WorkoutStreak)
            .WithName("30-Day Streak")
            .WithDescription("Complete workouts for 30 days in a row")
            .WithTarget(30)
            .WithTier(AchievementTier.Gold)
            .Build();

        /// <summary>
        /// Creates a "Total Workouts" bronze achievement.
        /// </summary>
        public static Achievement TotalWorkoutsBronze() => Achievement.CreateBuilder()
            .WithType(AchievementType.TotalWorkouts)
            .WithName("Beginner")
            .WithDescription("Complete 10 workouts")
            .WithTarget(10)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates a "Total Workouts" platinum achievement.
        /// </summary>
        public static Achievement TotalWorkoutsPlatinum() => Achievement.CreateBuilder()
            .WithType(AchievementType.TotalWorkouts)
            .WithName("Workout Master")
            .WithDescription("Complete 100 workouts")
            .WithTarget(100)
            .WithTier(AchievementTier.Platinum)
            .Build();

        /// <summary>
        /// Creates a "Total Workouts" titan achievement.
        /// </summary>
        public static Achievement TotalWorkoutsTitan() => Achievement.CreateBuilder()
            .WithType(AchievementType.TotalWorkouts)
            .WithName("Workout Legend")
            .WithDescription("Complete 1000 workouts")
            .WithTarget(1000)
            .WithTier(AchievementTier.Titan)
            .Build();

        #endregion

        #region Volume Achievements

        /// <summary>
        /// Creates a "Total Volume" bronze achievement.
        /// </summary>
        public static Achievement TotalVolumeBronze() => Achievement.CreateBuilder()
            .WithType(AchievementType.TotalVolume)
            .WithName("Volume Rookie")
            .WithDescription("Lift a total of 10,000 kg")
            .WithTarget(10000)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates a "Total Volume" gold achievement.
        /// </summary>
        public static Achievement TotalVolumeGold() => Achievement.CreateBuilder()
            .WithType(AchievementType.TotalVolume)
            .WithName("Volume King")
            .WithDescription("Lift a total of 100,000 kg")
            .WithTarget(100000)
            .WithTier(AchievementTier.Gold)
            .Build();

        /// <summary>
        /// Creates a "Total Volume" diamond achievement.
        /// </summary>
        public static Achievement TotalVolumeDiamond() => Achievement.CreateBuilder()
            .WithType(AchievementType.TotalVolume)
            .WithName("Volume Titan")
            .WithDescription("Lift a total of 1,000,000 kg")
            .WithTarget(1000000)
            .WithTier(AchievementTier.Diamond)
            .Build();

        #endregion

        #region Weight Achievements

        /// <summary>
        /// Creates a "Max Weight" bronze achievement.
        /// </summary>
        public static Achievement MaxWeightBronze() => Achievement.CreateBuilder()
            .WithType(AchievementType.MaxWeight)
            .WithName("Strong Start")
            .WithDescription("Lift 50 kg in a single rep")
            .WithTarget(50)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates a "Max Weight" gold achievement.
        /// </summary>
        public static Achievement MaxWeightGold() => Achievement.CreateBuilder()
            .WithType(AchievementType.MaxWeight)
            .WithName("Power Lifter")
            .WithDescription("Lift 100 kg in a single rep")
            .WithTarget(100)
            .WithTier(AchievementTier.Gold)
            .Build();

        /// <summary>
        /// Creates a "Max Weight" emerald achievement.
        /// </summary>
        public static Achievement MaxWeightEmerald() => Achievement.CreateBuilder()
            .WithType(AchievementType.MaxWeight)
            .WithName("Heavyweight Champion")
            .WithDescription("Lift 200 kg in a single rep")
            .WithTarget(200)
            .WithTier(AchievementTier.Emerald)
            .Build();

        /// <summary>
        /// Creates a "Weight Milestone" achievement.
        /// </summary>
        public static Achievement WeightMilestone() => Achievement.CreateBuilder()
            .WithType(AchievementType.WeightMilestone)
            .WithName("Weight Warrior")
            .WithDescription("Reach a specific weight milestone")
            .WithTarget(75)
            .WithTier(AchievementTier.Silver)
            .Build();

        #endregion

        #region Consistency Achievements

        /// <summary>
        /// Creates a "Consecutive Days" bronze achievement.
        /// </summary>
        public static Achievement ConsecutiveDaysBronze() => Achievement.CreateBuilder()
            .WithType(AchievementType.ConsecutiveDays)
            .WithName("Consistency Starter")
            .WithDescription("Train for 5 consecutive days")
            .WithTarget(5)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates a "Consecutive Days" platinum achievement.
        /// </summary>
        public static Achievement ConsecutiveDaysPlatinum() => Achievement.CreateBuilder()
            .WithType(AchievementType.ConsecutiveDays)
            .WithName("Iron Will")
            .WithDescription("Train for 100 consecutive days")
            .WithTarget(100)
            .WithTier(AchievementTier.Platinum)
            .Build();

        /// <summary>
        /// Creates a "Consecutive Days" titan achievement.
        /// </summary>
        public static Achievement ConsecutiveDaysTitan() => Achievement.CreateBuilder()
            .WithType(AchievementType.ConsecutiveDays)
            .WithName("Unstoppable Force")
            .WithDescription("Train for 365 consecutive days")
            .WithTarget(365)
            .WithTier(AchievementTier.Titan)
            .Build();

        #endregion

        #region Variety Achievements

        /// <summary>
        /// Creates an "Exercise Variety" bronze achievement.
        /// </summary>
        public static Achievement ExerciseVarietyBronze() => Achievement.CreateBuilder()
            .WithType(AchievementType.ExerciseVariety)
            .WithName("Well-Rounded")
            .WithDescription("Perform 10 different exercises")
            .WithTarget(10)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates an "Exercise Variety" gold achievement.
        /// </summary>
        public static Achievement ExerciseVarietyGold() => Achievement.CreateBuilder()
            .WithType(AchievementType.ExerciseVariety)
            .WithName("Exercise Expert")
            .WithDescription("Perform 50 different exercises")
            .WithTarget(50)
            .WithTier(AchievementTier.Gold)
            .Build();

        /// <summary>
        /// Creates an "Exercise Variety" diamond achievement.
        /// </summary>
        public static Achievement ExerciseVarietyDiamond() => Achievement.CreateBuilder()
            .WithType(AchievementType.ExerciseVariety)
            .WithName("Exercise Encyclopedia")
            .WithDescription("Perform 100 different exercises")
            .WithTarget(100)
            .WithTier(AchievementTier.Diamond)
            .Build();

        #endregion

        #region Quality Achievements

        /// <summary>
        /// Creates a "Perfect Form" achievement.
        /// </summary>
        public static Achievement PerfectForm() => Achievement.CreateBuilder()
            .WithType(AchievementType.PerfectForm)
            .WithName("Form Master")
            .WithDescription("Complete 50 workouts with perfect form")
            .WithTarget(50)
            .WithTier(AchievementTier.Gold)
            .Build();

        /// <summary>
        /// Creates a "Power Lifter" achievement.
        /// </summary>
        public static Achievement PowerLifter() => Achievement.CreateBuilder()
            .WithType(AchievementType.PowerLifter)
            .WithName("Power House")
            .WithDescription("Complete powerlifting milestones")
            .WithTarget(3)
            .WithTier(AchievementTier.Emerald)
            .Build();

        #endregion

        #region Time-Based Achievements

        /// <summary>
        /// Creates an "Early Bird" achievement.
        /// </summary>
        public static Achievement EarlyBird() => Achievement.CreateBuilder()
            .WithType(AchievementType.EarlyBird)
            .WithName("Morning Warrior")
            .WithDescription("Complete 20 workouts before 8 AM")
            .WithTarget(20)
            .WithTier(AchievementTier.Silver)
            .Build();

        /// <summary>
        /// Creates a "Night Owl" achievement.
        /// </summary>
        public static Achievement NightOwl() => Achievement.CreateBuilder()
            .WithType(AchievementType.NightOwl)
            .WithName("Night Grinder")
            .WithDescription("Complete 20 workouts after 8 PM")
            .WithTarget(20)
            .WithTier(AchievementTier.Silver)
            .Build();

        #endregion

        #region Reps Achievements

        /// <summary>
        /// Creates a "Reps Milestone" bronze achievement.
        /// </summary>
        public static Achievement RepsMilestoneBronze() => Achievement.CreateBuilder()
            .WithType(AchievementType.RepsMilestone)
            .WithName("Rep Starter")
            .WithDescription("Complete 1,000 total reps")
            .WithTarget(1000)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates a "Reps Milestone" platinum achievement.
        /// </summary>
        public static Achievement RepsMilestonePlatinum() => Achievement.CreateBuilder()
            .WithType(AchievementType.RepsMilestone)
            .WithName("Rep Master")
            .WithDescription("Complete 100,000 total reps")
            .WithTarget(100000)
            .WithTier(AchievementTier.Platinum)
            .Build();

        #endregion

        #region Custom Achievements

        /// <summary>
        /// Creates an achievement with specified user ID.
        /// </summary>
        public static Achievement WithUserId(Guid userId) => Achievement.CreateBuilder()
            .WithUserId(userId)
            .WithType(AchievementType.FirstWorkout)
            .WithName("User Achievement")
            .WithDescription("Achievement for specific user")
            .WithTarget(50)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates an achievement with specified type.
        /// </summary>
        public static Achievement WithType(AchievementType type) => Achievement.CreateBuilder()
            .WithType(type)
            .WithName($"{type} Achievement")
            .WithDescription($"Achievement for {type}")
            .WithTarget(50)
            .WithTier(AchievementTier.Bronze)
            .Build();

        /// <summary>
        /// Creates an achievement with specified tier.
        /// </summary>
        public static Achievement WithTier(AchievementTier tier) => Achievement.CreateBuilder()
            .WithType(AchievementType.TotalWorkouts)
            .WithName($"{tier} Achievement")
            .WithDescription($"{tier} tier achievement")
            .WithTarget(tier switch
            {
                AchievementTier.Bronze => 10,
                AchievementTier.Silver => 25,
                AchievementTier.Gold => 50,
                AchievementTier.Platinum => 100,
                AchievementTier.Diamond => 250,
                AchievementTier.Emerald => 500,
                AchievementTier.Titan => 1000,
                _ => 100
            })
            .WithTier(tier)
            .Build();

        /// <summary>
        /// Creates an unlocked achievement.
        /// </summary>
        public static Achievement Unlocked()
        {
            var achievement = FirstWorkout();
            achievement.UpdateProgress(1);
            return achievement;
        }

        /// <summary>
        /// Creates an achievement with 50% progress.
        /// </summary>
        public static Achievement HalfwayComplete()
        {
            var achievement = Default();
            achievement.UpdateProgress(50);
            return achievement;
        }

        /// <summary>
        /// Creates an achievement almost complete (99%).
        /// </summary>
        public static Achievement AlmostComplete()
        {
            var achievement = Default();
            achievement.UpdateProgress(99);
            return achievement;
        }

        #endregion

        #region Collections

        /// <summary>
        /// Creates a collection of bronze tier achievements.
        /// </summary>
        public static List<Achievement> BronzeCollection() => new List<Achievement>
        {
            FirstWorkout(),
            WorkoutStreakBronze(),
            TotalWorkoutsBronze(),
            TotalVolumeBronze(),
            MaxWeightBronze(),
            ConsecutiveDaysBronze(),
            ExerciseVarietyBronze(),
            RepsMilestoneBronze()
        };

        /// <summary>
        /// Creates a collection of all tier achievements.
        /// </summary>
        public static List<Achievement> AllTiersCollection() => new List<Achievement>
        {
            FirstWorkout(),                 // Bronze
            WorkoutStreakSilver(),          // Silver
            WorkoutStreakGold(),            // Gold
            TotalWorkoutsPlatinum(),        // Platinum
            TotalVolumeDiamond(),           // Diamond
            MaxWeightEmerald(),             // Emerald
            ConsecutiveDaysTitan()          // Titan
        };

        /// <summary>
        /// Creates a progression path for workout count.
        /// </summary>
        public static List<Achievement> WorkoutProgressionPath() => new List<Achievement>
        {
            FirstWorkout(),                 // 1 workout
            TotalWorkoutsBronze(),          // 10 workouts
            TotalWorkoutsPlatinum(),        // 100 workouts
            TotalWorkoutsTitan()            // 1000 workouts
        };

        /// <summary>
        /// Creates a progression path for workout streaks.
        /// </summary>
        public static List<Achievement> StreakProgressionPath() => new List<Achievement>
        {
            WorkoutStreakBronze(),          // 3 days
            WorkoutStreakSilver(),          // 7 days
            WorkoutStreakGold()             // 30 days
        };

        /// <summary>
        /// Creates a collection with mixed progress.
        /// </summary>
        public static List<Achievement> MixedProgressCollection()
        {
            var unlocked = FirstWorkout();
            unlocked.UpdateProgress(1);

            var halfway = WorkoutStreakSilver();
            halfway.UpdateProgress(3);

            var almostDone = TotalWorkoutsBronze();
            almostDone.UpdateProgress(9);

            return new List<Achievement>
            {
                unlocked,
                halfway,
                almostDone,
                MaxWeightBronze()
            };
        }

        /// <summary>
        /// Creates a collection for specific user.
        /// </summary>
        public static List<Achievement> ForUser(Guid userId) => new List<Achievement>
        {
            Achievement.CreateBuilder().WithUserId(userId).WithType(AchievementType.FirstWorkout).WithName("First Workout").WithTarget(1).WithTier(AchievementTier.Bronze).Build(),
            Achievement.CreateBuilder().WithUserId(userId).WithType(AchievementType.TotalWorkouts).WithName("Total 10").WithTarget(10).WithTier(AchievementTier.Bronze).Build(),
            Achievement.CreateBuilder().WithUserId(userId).WithType(AchievementType.WorkoutStreak).WithName("3 Day Streak").WithTarget(3).WithTier(AchievementTier.Bronze).Build(),
            Achievement.CreateBuilder().WithUserId(userId).WithType(AchievementType.TotalVolume).WithName("Volume 10k").WithTarget(10000).WithTier(AchievementTier.Bronze).Build()
        };

        /// <summary>
        /// Creates all achievements for a complete achievement system.
        /// </summary>
        public static List<Achievement> CompleteAchievementSystem() => new List<Achievement>
        {
            // Workout milestones
            FirstWorkout(),
            TotalWorkoutsBronze(),
            TotalWorkoutsPlatinum(),
            TotalWorkoutsTitan(),

            // Streaks
            WorkoutStreakBronze(),
            WorkoutStreakSilver(),
            WorkoutStreakGold(),

            // Volume
            TotalVolumeBronze(),
            TotalVolumeGold(),
            TotalVolumeDiamond(),

            // Strength
            MaxWeightBronze(),
            MaxWeightGold(),
            MaxWeightEmerald(),

            // Consistency
            ConsecutiveDaysBronze(),
            ConsecutiveDaysPlatinum(),
            ConsecutiveDaysTitan(),

            // Variety
            ExerciseVarietyBronze(),
            ExerciseVarietyGold(),
            ExerciseVarietyDiamond(),

            // Quality & Time
            PerfectForm(),
            PowerLifter(),
            EarlyBird(),
            NightOwl(),

            // Reps
            RepsMilestoneBronze(),
            RepsMilestonePlatinum()
        };

        #endregion
    }
}
