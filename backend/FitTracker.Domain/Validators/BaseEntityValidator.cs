using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
    public class BaseEntityValidator<T> : AbstractValidator<T> where T : BaseEntity
    {
        public BaseEntityValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID cannot be empty");
            RuleFor(x => x.CreatedAt).NotEmpty();
            RuleFor(x => x.UpdatedAt).NotEmpty();
        }
    }
}
