using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise that can be performed in a workout.
    /// </summary>
    public class Exercise : BaseEntity
    {
        #region Constants

        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 1000;
        public const int MuscleGroupMaxLength = 50;
        public const int EquipmentMaxLength = 50;
        public const int ImageUrlMaxLength = 500;
        public const int VideoUrlMaxLength = 500;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the exercise.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the description of the exercise.
        /// </summary>
        public string? Description
        {
            get; private set;
        }

        /// <summary>
        /// Gets the URL to the exercise's image.
        /// </summary>
        public string? ImageUrl
        {
            get; private set;
        }

        /// <summary>
        /// Gets the URL to the exercise's instructional video.
        /// </summary>
        public string? VideoUrl
        {
            get; private set;
        }

        /// <summary>
        /// Gets the primary muscle group targeted by this exercise.
        /// </summary>
        public MuscleGroup MuscleGroup
        {
            get; private set;
        }

        /// <summary>
        /// Gets the equipment required for this exercise.
        /// </summary>
        public Equipment Equipment
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether this is a custom exercise created by a user.
        /// </summary>
        public bool IsCustom
        {
            get; private set;
        }

        /// <summary>
        /// Gets the unique identifier of the user who created this custom exercise, or null if this is a standard exercise.
        /// </summary>
        public Guid? UserId
        {
            get; private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private Exercise()
        {
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new exercises.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        private Exercise(
            string name,
            MuscleGroup muscleGroup,
            Equipment equipment,
            string? description = null,
            string? imageUrl = null,
            string? videoUrl = null,
            bool isCustom = false,
            Guid? userId = null) : base()
        {
            Name = name;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            Description = description;
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
            IsCustom = isCustom;
            UserId = userId;

            EnsureValid();
        }

        /// <summary>
        /// Constructor for restoring exercise from persistence layer.
        /// Use <see cref="ExerciseBuilder"/> for creating new exercises.
        /// </summary>
        public Exercise(
            string name,
            string? description,
            string? imageUrl,
            string? videoUrl,
            MuscleGroup muscleGroup,
            Equipment equipment,
            bool isCustom,
            Guid? userId) : base()
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            IsCustom = isCustom;
            UserId = userId;

            // No validation here since data is from persistence
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new ExerciseValidator();
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Updates the core properties of the exercise.
        /// </summary>
        /// <param name="name">The new name for the exercise.</param>
        /// <param name="muscleGroup">The new target muscle group.</param>
        /// <param name="equipment">The new required equipment.</param>
        /// <param name="description">The new description (optional).</param>
        /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
        public void Update(string name, MuscleGroup muscleGroup, Equipment equipment, string? description = null)
        {

            Name = name;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            Description = description;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Updates the image URL of the exercise.
        /// </summary>
        /// <param name="imageUrl">The new image URL.</param>
        public void UpdateImageUrl(string? imageUrl)
        {
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the video URL of the exercise.
        /// </summary>
        /// <param name="videoUrl">The new video URL.</param>
        public void UpdateVideoUrl(string? videoUrl)
        {
            VideoUrl = videoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="ExerciseBuilder"/> instance.
        /// </summary>
        public static ExerciseBuilder CreateBuilder()
        {
            return new ExerciseBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="Exercise"/> instances.
        /// </summary>
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

            /// <summary>
            /// Builds the <see cref="Exercise"/> entity.
            /// </summary>
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
                    _userId);
            }
        }

        #endregion
    }
}
