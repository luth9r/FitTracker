using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise that can be performed as part of a workout.
    /// </summary>
    public class Exercise : BaseEntity
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 1000;
        public const int MuscleGroupMaxLength = 50;
        public const int EquipmentMaxLength = 50;
        public const int ImageUrlMaxLength = 500;
        public const int VideoUrlMaxLength = 500;

        public string Name { get; private set; } = default!;

        public string? Description { get; private set; }

        public string? ImageUrl { get; private set; }

        public string? VideoUrl { get; private set; }

        public MuscleGroup MuscleGroup { get; private set; }

        public Equipment Equipment { get; private set; }

        public Guid? CreatedByUserId { get; private set; }

        public bool IsCustomExercise() => CreatedByUserId.HasValue;

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

        private Exercise()
        {
        }

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

        public void UpdateMedia(string? imageUrl, string? videoUrl)
        {
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
