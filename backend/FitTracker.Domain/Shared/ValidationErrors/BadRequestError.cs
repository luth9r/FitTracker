using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Domain.Shared.ValidationErrors
{
    public class BadRequestError(IEnumerable<ValidationFailure> failures) : ValidationResult(failures);
}
