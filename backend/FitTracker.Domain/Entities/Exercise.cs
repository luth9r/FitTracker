using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise that can be performed as part of a workout.
    /// </summary>
    public class Exercise : BaseEntity
    {
        /// <summary>
        /// The maximum length allowed for the exercise name.
        /// </summary>
        public const int NameMaxLength = 100;
        /// <summary>
        /// The maximum length allowed for the exercise description.
        /// </summary>
        public const int DescriptionMaxLength = 1000;
        /// <summary>
        /// The maximum length allowed for the muscle group name.
        /// </summary>
        public const int MuscleGroupMaxLength = 50;
        /// <summary>
        /// The maximum length allowed for the equipment name.
        /// </summary>
        public const int EquipmentMaxLength = 50;
        /// <summary>
        /// The maximum length allowed for the image URL.
        /// </summary>
        public const int ImageUrlMaxLength = 500;
        /// <summary>
        /// The maximum length allowed for the video URL.
        /// </summary>
        public const int VideoUrlMaxLength = 500;

        /// <summary>
        /// Gets the name of the exercise.
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets the description of the exercise.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Gets the URL of the exercise image.
        /// </summary>
        public string? ImageUrl { get; private set; }

        /// <summary>
        /// Gets the URL of the exercise video.
        /// </summary>
        public string? VideoUrl { get; private set; }

        /// <summary>
        /// Gets the muscle group targeted by the exercise.
        /// </summary>
        public MuscleGroup MuscleGroup { get; private set; }

        /// <summary>
        /// Gets the equipment required for the exercise.
        /// </summary>
        public Equipment Equipment { get; private set; }

        /// <summary>
        /// Gets the ID of the user who created the exercise, or null if it's a standard system exercise.
        /// </summary>
        public Guid? CreatedByUserId { get; private set; }

        /// <summary>
        /// Determines whether the exercise is a custom exercise created by a user.
        /// </summary>
        /// <returns><c>true</c> if the exercise is custom; otherwise, <c>false</c>.</returns>
        public bool IsCustomExercise() => CreatedByUserId.HasValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Exercise"/> class.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="name">The name of the exercise.</param>
        /// <param name="description">The description of the exercise.</param>
        /// <param name="imageUrl">The URL of the exercise image.</param>
        /// <param name="videoUrl">The URL of the exercise video.</param>
        /// <param name="muscleGroup">The muscle group targeted by the exercise.</param>
        /// <param name="equipment">The equipment required for the exercise.</param>
        /// <param name="createdByUserId">The ID of the user who created the exercise.</param>
        /// <param name="createdAt">The date and time of creation.</param>
        /// <param name="updatedAt">The date and time of the last update.</param>
        internal Exercise(
            Guid id,
            string name,
            string? description,
            string? imageUrl,
            string? videoUrl,
            MuscleGroup muscleGroup,
            Equipment equipment,
            Guid? createdByUserId,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            CreatedByUserId = createdByUserId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exercise"/> class.
        /// </summary>
        private Exercise()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exercise"/> class.
        /// </summary>
        /// <param name="name">The name of the exercise.</param>
        /// <param name="muscleGroup">The muscle group targeted by the exercise.</param>
        /// <param name="equipment">The equipment required for the exercise.</param>
        /// <param name="description">The description of the exercise.</param>
        /// <param name="imageUrl">The URL of the exercise image.</param>
        /// <param name="videoUrl">The URL of the exercise video.</param>
        /// <param name="createdByUserId">The ID of the user who created the exercise.</param>
        private Exercise(
            string name,
            MuscleGroup muscleGroup,
            Equipment equipment,
            string? description = null,
            string? imageUrl = null,
            string? videoUrl = null,
            Guid? createdByUserId = null)
        {
            // Guard clauses
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required", nameof(name));
            }

            if (name.Length > NameMaxLength)
            {
                throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters", nameof(name));
            }

            if (description?.Length > DescriptionMaxLength)
            {
                throw new ArgumentException($"Description cannot exceed {DescriptionMaxLength} characters", nameof(description));
            }

            Name = name;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            Description = description;
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
            CreatedByUserId = createdByUserId;
        }

        /// <summary>
        /// Creates a new standard system exercise.
        /// </summary>
        /// <param name="name">The name of the exercise.</param>
        /// <param name="muscleGroup">The muscle group targeted by the exercise.</param>
        /// <param name="equipment">The equipment required for the exercise.</param>
        /// <param name="description">The description of the exercise.</param>
        /// <param name="imageUrl">The URL of the exercise image.</param>
        /// <param name="videoUrl">The URL of the exercise video.</param>
        /// <returns>A new instance of the <see cref="Exercise"/> class.</returns>
        public static Exercise CreateStandard(
            string name,
            MuscleGroup muscleGroup,
            Equipment equipment,
            string? description = null,
            string? imageUrl = null,
            string? videoUrl = null)
        {
            return new Exercise(name, muscleGroup, equipment, description, imageUrl, videoUrl, null);
        }

        /// <summary>
        /// Creates a new custom exercise for a user.
        /// </summary>
        /// <param name="userId">The ID of the user creating the exercise.</param>
        /// <param name="name">The name of the exercise.</param>
        /// <param name="muscleGroup">The muscle group targeted by the exercise.</param>
        /// <param name="equipment">The equipment required for the exercise.</param>
        /// <param name="description">The description of the exercise.</param>
        /// <param name="imageUrl">The URL of the exercise image.</param>
        /// <param name="videoUrl">The URL of the exercise video.</param>
        /// <returns>A new instance of the <see cref="Exercise"/> class.</returns>
        public static Exercise CreateCustom(
            Guid userId,
            string name,
            MuscleGroup muscleGroup,
            Equipment equipment,
            string? description = null,
            string? imageUrl = null,
            string? videoUrl = null)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty for custom exercise", nameof(userId));
            }

            return new Exercise(name, muscleGroup, equipment, description, imageUrl, videoUrl, userId);
        }

        /// <summary>
        /// Updates the exercise details. Only applicable for custom exercises.
        /// </summary>
        /// <param name="name">The new name of the exercise.</param>
        /// <param name="muscleGroup">The new muscle group of the exercise.</param>
        /// <param name="equipment">The new equipment required for the exercise.</param>
        /// <param name="description">The new description of the exercise.</param>
        public void Update(string name, MuscleGroup muscleGroup, Equipment equipment, string? description = null)
        {
            if (!IsCustomExercise())
            {
                throw new InvalidOperationException("Cannot update standard exercises");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required", nameof(name));
            }

            if (name.Length > NameMaxLength)
            {
                throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters", nameof(name));
            }

            Name = name;
            MuscleGroup = muscleGroup;
            Equipment = equipment;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the media URLS of the exercise.
        /// </summary>
        /// <param name="imageUrl">The new image URL.</param>
        /// <param name="videoUrl">The new video URL.</param>
        public void UpdateMedia(string? imageUrl, string? videoUrl)
        {
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
