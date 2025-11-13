using CSharpFunctionalExtensions;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise that can be performed as part of a workout.
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

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string? ImageUrl { get; private set; }
        public string? VideoUrl { get; private set; }
        public MuscleGroup MuscleGroup { get; private set; }
        public Equipment Equipment { get; private set; }
        public bool IsCustom { get; private set; }
        public Guid? UserId { get; private set; }

        #endregion

        #region Constructors

        private Exercise()
        {
            // For ORM
        }

        public Exercise(
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
        }

        public Exercise(
            string name,
            string? description,
            string? imageUrl,
            string? videoUrl,
            MuscleGroup muscleGroup,
            Equipment equipment,
            bool isCustom,
            Guid? userId) : this(name, muscleGroup, equipment, description, imageUrl, videoUrl, isCustom, userId)
        {
            // No validation here as this typically restores from persistence
        }

        #endregion

        #region Factory Method with Validation

        public static Result<Exercise, ValidationResult> Create(
            string name,
            MuscleGroup muscleGroup,
            Equipment equipment,
            string? description = null,
            string? imageUrl = null,
            string? videoUrl = null,
            bool isCustom = false,
            Guid? userId = null)
        {
            var exercise = new Exercise(name, muscleGroup, equipment, description, imageUrl, videoUrl, isCustom, userId);
            return exercise.ValidateWithResult();
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator() => new ExerciseValidator();

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<object>(this));
        }

        private Result<Exercise, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<Exercise, ValidationResult>(result);

            return Result.Success<Exercise, ValidationResult>(this);
        }

        #endregion

        #region Domain Methods

        public Result<Exercise, ValidationResult> Update(string name, MuscleGroup muscleGroup, Equipment equipment, string? description = null)
        {
            Name = name;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            Description = description;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public void UpdateImageUrl(string? imageUrl)
        {
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateVideoUrl(string? videoUrl)
        {
            VideoUrl = videoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Builder

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

            public ExerciseBuilder WithName(string name) { _name = name; return this; }
            public ExerciseBuilder WithDescription(string? description) { _description = description; return this; }
            public ExerciseBuilder WithImageUrl(string? imageUrl) { _imageUrl = imageUrl; return this; }
            public ExerciseBuilder WithVideoUrl(string? videoUrl) { _videoUrl = videoUrl; return this; }
            public ExerciseBuilder WithMuscleGroup(MuscleGroup muscleGroup) { _muscleGroup = muscleGroup; return this; }
            public ExerciseBuilder WithEquipment(Equipment equipment) { _equipment = equipment; return this; }
            public ExerciseBuilder AsCustom(Guid userId) { _isCustom = true; _userId = userId; return this; }
            public ExerciseBuilder AsStandard() { _isCustom = false; _userId = null; return this; }

            public Result<Exercise, ValidationResult> Build()
            {
                var exercise = new Exercise(
                    _name,
                    _muscleGroup,
                    _equipment,
                    _description,
                    _imageUrl,
                    _videoUrl,
                    _isCustom,
                    _userId);

                var validationResult = exercise.Validate();

                if (!validationResult.IsValid)
                    return Result.Failure<Exercise, ValidationResult>(validationResult);

                return Result.Success<Exercise, ValidationResult>(exercise);
            }
        }

        #endregion
    }
}
