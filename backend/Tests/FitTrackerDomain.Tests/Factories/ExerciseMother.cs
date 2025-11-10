using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;

namespace FitTrackerDomain.Tests.Factories
{
    /// <summary>
    /// Factory for creating Exercise test data.
    /// </summary>
    public static class ExerciseMother
    {
        /// <summary>
        /// Creates a default standard exercise.
        /// </summary>
        public static Exercise Default() => Exercise.CreateBuilder()
            .WithName("Bench Press")
            .WithDescription("Standard bench press exercise")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Barbell)
            .AsStandard()
            .Build();

        #region Chest Exercises

        /// <summary>
        /// Creates a barbell bench press exercise.
        /// </summary>
        public static Exercise BenchPress() => Exercise.CreateBuilder()
            .WithName("Barbell Bench Press")
            .WithDescription("Flat barbell bench press targeting chest, shoulders, and triceps")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Barbell)
            .WithImageUrl("https://example.com/images/bench-press.jpg")
            .WithVideoUrl("https://example.com/videos/bench-press.mp4")
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a dumbbell chest fly exercise.
        /// </summary>
        public static Exercise DumbbellChestFly() => Exercise.CreateBuilder()
            .WithName("Dumbbell Chest Fly")
            .WithDescription("Isolation exercise for chest muscles")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Dumbbell)
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a push-up exercise.
        /// </summary>
        public static Exercise PushUp() => Exercise.CreateBuilder()
            .WithName("Push-Up")
            .WithDescription("Bodyweight chest and triceps exercise")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Bodyweight)
            .AsStandard()
            .Build();

        #endregion

        #region Back Exercises

        /// <summary>
        /// Creates a barbell deadlift exercise.
        /// </summary>
        public static Exercise Deadlift() => Exercise.CreateBuilder()
            .WithName("Barbell Deadlift")
            .WithDescription("Compound movement targeting back, legs, and core")
            .WithMuscleGroup(MuscleGroup.Back)
            .WithEquipment(Equipment.Barbell)
            .WithImageUrl("https://example.com/images/deadlift.jpg")
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a pull-up exercise.
        /// </summary>
        public static Exercise PullUp() => Exercise.CreateBuilder()
            .WithName("Pull-Up")
            .WithDescription("Bodyweight back exercise")
            .WithMuscleGroup(MuscleGroup.Back)
            .WithEquipment(Equipment.Bodyweight)
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a cable row exercise.
        /// </summary>
        public static Exercise CableRow() => Exercise.CreateBuilder()
            .WithName("Cable Row")
            .WithDescription("Machine-based rowing exercise")
            .WithMuscleGroup(MuscleGroup.Back)
            .WithEquipment(Equipment.Cable)
            .AsStandard()
            .Build();

        #endregion

        #region Leg Exercises

        /// <summary>
        /// Creates a barbell squat exercise.
        /// </summary>
        public static Exercise Squat() => Exercise.CreateBuilder()
            .WithName("Barbell Squat")
            .WithDescription("Compound leg exercise")
            .WithMuscleGroup(MuscleGroup.Legs)
            .WithEquipment(Equipment.Barbell)
            .WithImageUrl("https://example.com/images/squat.jpg")
            .WithVideoUrl("https://example.com/videos/squat.mp4")
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a leg press exercise.
        /// </summary>
        public static Exercise LegPress() => Exercise.CreateBuilder()
            .WithName("Leg Press")
            .WithDescription("Machine-based leg exercise")
            .WithMuscleGroup(MuscleGroup.Legs)
            .WithEquipment(Equipment.Machine)
            .AsStandard()
            .Build();

        #endregion

        #region Shoulder Exercises

        /// <summary>
        /// Creates a shoulder press exercise.
        /// </summary>
        public static Exercise ShoulderPress() => Exercise.CreateBuilder()
            .WithName("Dumbbell Shoulder Press")
            .WithDescription("Overhead pressing movement for shoulders")
            .WithMuscleGroup(MuscleGroup.Shoulders)
            .WithEquipment(Equipment.Dumbbell)
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a lateral raise exercise.
        /// </summary>
        public static Exercise LateralRaise() => Exercise.CreateBuilder()
            .WithName("Dumbbell Lateral Raise")
            .WithDescription("Isolation exercise for side deltoids")
            .WithMuscleGroup(MuscleGroup.Shoulders)
            .WithEquipment(Equipment.Dumbbell)
            .AsStandard()
            .Build();

        #endregion

        #region Arm Exercises

        /// <summary>
        /// Creates a barbell curl exercise.
        /// </summary>
        public static Exercise BarbellCurl() => Exercise.CreateBuilder()
            .WithName("Barbell Curl")
            .WithDescription("Bicep isolation exercise")
            .WithMuscleGroup(MuscleGroup.Biceps)
            .WithEquipment(Equipment.Barbell)
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a tricep dip exercise.
        /// </summary>
        public static Exercise TricepDip() => Exercise.CreateBuilder()
            .WithName("Tricep Dip")
            .WithDescription("Bodyweight tricep exercise")
            .WithMuscleGroup(MuscleGroup.Triceps)
            .WithEquipment(Equipment.Bodyweight)
            .AsStandard()
            .Build();

        #endregion

        #region Core Exercises

        /// <summary>
        /// Creates a plank exercise.
        /// </summary>
        public static Exercise Plank() => Exercise.CreateBuilder()
            .WithName("Plank")
            .WithDescription("Core stability exercise")
            .WithMuscleGroup(MuscleGroup.Abs)
            .WithEquipment(Equipment.Bodyweight)
            .AsStandard()
            .Build();

        #endregion

        #region Custom Exercises

        /// <summary>
        /// Creates a custom exercise for a specific user.
        /// </summary>
        public static Exercise CustomExercise(Guid userId) => Exercise.CreateBuilder()
            .WithName("My Custom Exercise")
            .WithDescription("User-created custom exercise")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Dumbbell)
            .AsCustom(userId)
            .Build();

        /// <summary>
        /// Creates a custom exercise with all fields.
        /// </summary>
        public static Exercise CustomWithAllFields(Guid userId) => Exercise.CreateBuilder()
            .WithName("Custom Chest Movement")
            .WithDescription("My personalized chest exercise")
            .WithImageUrl("https://example.com/my-exercise.jpg")
            .WithVideoUrl("https://example.com/my-exercise.mp4")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Dumbbell)
            .AsCustom(userId)
            .Build();

        #endregion

        #region Exercises by Equipment

        /// <summary>
        /// Creates a barbell exercise.
        /// </summary>
        public static Exercise WithBarbell() => Exercise.CreateBuilder()
            .WithName("Barbell Exercise")
            .WithDescription("Exercise using barbell")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Barbell)
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a dumbbell exercise.
        /// </summary>
        public static Exercise WithDumbbell() => Exercise.CreateBuilder()
            .WithName("Dumbbell Exercise")
            .WithDescription("Exercise using dumbbells")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Dumbbell)
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a bodyweight exercise.
        /// </summary>
        public static Exercise WithBodyweight() => Exercise.CreateBuilder()
            .WithName("Bodyweight Exercise")
            .WithDescription("Exercise using bodyweight only")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Bodyweight)
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a machine exercise.
        /// </summary>
        public static Exercise WithMachine() => Exercise.CreateBuilder()
            .WithName("Machine Exercise")
            .WithDescription("Exercise using machine")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Machine)
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates a cable exercise.
        /// </summary>
        public static Exercise WithCable() => Exercise.CreateBuilder()
            .WithName("Cable Exercise")
            .WithDescription("Exercise using cable machine")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Cable)
            .AsStandard()
            .Build();

        #endregion

        #region Exercises with Media

        /// <summary>
        /// Creates an exercise with image only.
        /// </summary>
        public static Exercise WithImage() => Exercise.CreateBuilder()
            .WithName("Exercise with Image")
            .WithDescription("Has image but no video")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Barbell)
            .WithImageUrl("https://example.com/exercise-image.jpg")
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates an exercise with video only.
        /// </summary>
        public static Exercise WithVideo() => Exercise.CreateBuilder()
            .WithName("Exercise with Video")
            .WithDescription("Has video but no image")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Barbell)
            .WithVideoUrl("https://example.com/exercise-video.mp4")
            .AsStandard()
            .Build();

        /// <summary>
        /// Creates an exercise with both image and video.
        /// </summary>
        public static Exercise WithImageAndVideo() => Exercise.CreateBuilder()
            .WithName("Exercise with Media")
            .WithDescription("Has both image and video")
            .WithMuscleGroup(MuscleGroup.Chest)
            .WithEquipment(Equipment.Barbell)
            .WithImageUrl("https://example.com/exercise-image.jpg")
            .WithVideoUrl("https://example.com/exercise-video.mp4")
            .AsStandard()
            .Build();

        #endregion

        #region Collections

        /// <summary>
        /// Creates a collection of chest exercises.
        /// </summary>
        public static List<Exercise> ChestExercises() => new List<Exercise>
        {
            BenchPress(),
            DumbbellChestFly(),
            PushUp()
        };

        /// <summary>
        /// Creates a collection of back exercises.
        /// </summary>
        public static List<Exercise> BackExercises() => new List<Exercise>
        {
            Deadlift(),
            PullUp(),
            CableRow()
        };

        /// <summary>
        /// Creates a collection of compound exercises.
        /// </summary>
        public static List<Exercise> CompoundExercises() => new List<Exercise>
        {
            BenchPress(),
            Deadlift(),
            Squat()
        };

        /// <summary>
        /// Creates a collection of bodyweight exercises.
        /// </summary>
        public static List<Exercise> BodyweightExercises() => new List<Exercise>
        {
            PushUp(),
            PullUp(),
            TricepDip(),
            Plank()
        };

        /// <summary>
        /// Creates a collection of all standard exercises.
        /// </summary>
        public static List<Exercise> AllStandardExercises() => new List<Exercise>
        {
            BenchPress(),
            DumbbellChestFly(),
            PushUp(),
            Deadlift(),
            PullUp(),
            CableRow(),
            Squat(),
            LegPress(),
            ShoulderPress(),
            LateralRaise(),
            BarbellCurl(),
            TricepDip(),
            Plank()
        };

        /// <summary>
        /// Creates a mixed collection with custom exercises.
        /// </summary>
        public static List<Exercise> MixedCollection(Guid userId) => new List<Exercise>
        {
            BenchPress(),
            Squat(),
            CustomExercise(userId),
            PullUp()
        };

        #endregion
    }
}
