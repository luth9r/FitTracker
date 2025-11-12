using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;

namespace FitTracker.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id
        {
            get; protected set;
        }
        public DateTime CreatedAt
        {
            get; protected set;
        }
        public DateTime UpdatedAt
        {
            get; protected set;
        }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDatabaseFields(Guid id, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        protected abstract IValidator GetValidator();

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<object>(this));
        }

        public Result<BaseEntity, ValidationResult> ValidateWithResult()
        {
            var validationResult = Validate();

            if (!validationResult.IsValid)
                return Result.Failure<BaseEntity, ValidationResult>(validationResult);

            return Result.Success<BaseEntity, ValidationResult>(this);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}
