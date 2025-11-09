using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FitTracker.Domain.Validators;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise that can be performed in a workout
    /// </summary>
    public class Exercise : BaseEntity
    {
        // ============================================
        // Constants
        // ============================================
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 1000;
        public const int MuscleGroupMaxLength = 50;
        public const int EquipmentMaxLength = 50;
        public const int ImageUrlMaxLength = 500;
        public const int VideoUrlMaxLength = 500;

        // ============================================
        // Properties
        // ============================================
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string? ImageUrl { get; private set; }
        public string? VideoUrl { get; private set; }
        public MuscleGroup MuscleGroup { get; private set; }
        public Equipment Equipment { get; private set; }
        public bool IsCustom { get; private set; }
        public Guid? UserId { get; private set; }

        // Navigation Properties
        public User? User { get; private set; }
        public ICollection<WorkoutExercise> WorkoutExercises { get; private set; }

        // ============================================
        // Constructors
        // ============================================

        /// <summary>
        /// EF Core constructor
        /// </summary>
        private Exercise()
        {
            Name = string.Empty;
            MuscleGroup = MuscleGroup.Chest;
            Equipment = Equipment.Barbell;
            WorkoutExercises = new HashSet<WorkoutExercise>();
        }

        public Exercise(
            string name,
            string? description,
            string? imageUrl,
            string? videoUrl,
            MuscleGroup muscleGroup,
            Equipment equipment,
            bool isCustom,
            Guid? userId)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            IsCustom = isCustom;
            UserId = userId;
            WorkoutExercises = new HashSet<WorkoutExercise>();
        }



        /// <summary>
        /// Domain constructor
        /// </summary>
        private Exercise(
            string name,
            MuscleGroup muscleGroup,
            Equipment equipment,
            string? description = null,
            string? imageUrl = null,
            string? videoUrl = null,
            bool isCustom = false,
            Guid? userId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Exercise name cannot be empty");

            if (isCustom && userId == null)
                throw new ArgumentException("Custom exercises must have a user ID");

            Name = name;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            Description = description;
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
            IsCustom = isCustom;
            UserId = userId;
            WorkoutExercises = new HashSet<WorkoutExercise>();

            EnsureValid();
        }

        // ============================================
        // Validator
        // ============================================
        protected override IValidator GetValidator()
        {
            return new ExerciseValidator();
        }

        // ============================================
        // Builder Pattern
        // ============================================

        public static ExerciseBuilder CreateBuilder() => new ExerciseBuilder();

        public class ExerciseBuilder
        {
            private string _name = string.Empty;
            private string? _description;
            private string? _imageUrl;
            private string? _videoUrl;
            private MuscleGroup _muscleGroup = MuscleGroup.Chest;
            private Equipment _equipment = Equipment.Barbell;
            private bool _isCustom;
            private Guid? _userId;

            public ExerciseBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ExerciseBuilder WithDescription(string? description)
            {
                _description = description;
                return this;
            }

            public ExerciseBuilder WithImageUrl(string? imageUrl)
            {
                _imageUrl = imageUrl;
                return this;
            }

            public ExerciseBuilder WithVideoUrl(string? videoUrl)
            {
                _videoUrl = videoUrl;
                return this;
            }

            public ExerciseBuilder WithMuscleGroup(MuscleGroup muscleGroup)
            {
                _muscleGroup = muscleGroup;
                return this;
            }

            public ExerciseBuilder WithEquipment(Equipment equipment)
            {
                _equipment = equipment;
                return this;
            }

            public ExerciseBuilder AsCustom(Guid userId)
            {
                _isCustom = true;
                _userId = userId;
                return this;
            }

            public ExerciseBuilder AsStandard()
            {
                _isCustom = false;
                _userId = null;
                return this;
            }

            public Exercise Build()
            {
                return new Exercise(
                    _name,
                    _muscleGroup,
                    _equipment,
                    _description,
                    _imageUrl,
                    _videoUrl,
                    _isCustom,
                    _userId
                );
            }
        }

        // ============================================
        // Domain Methods
        // ============================================

        /// <summary>
        /// Update exercise details
        /// </summary>
        public void Update(
            string name,
            MuscleGroup muscleGroup,
            Equipment equipment,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Exercise name cannot be empty");

            Name = name;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            Description = description;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Update image URL
        /// </summary>
        public void UpdateImageUrl(string? imageUrl)
        {
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Update video URL
        /// </summary>
        public void UpdateVideoUrl(string? videoUrl)
        {
            VideoUrl = videoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Check if exercise can be deleted
        /// </summary>
        public bool CanBeDeleted()
        {
            if (!IsCustom)
                return false;

            return !WorkoutExercises.Any();
        }
    }
}
