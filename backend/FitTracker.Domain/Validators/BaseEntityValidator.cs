using FitTracker.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
