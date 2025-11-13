using FluentValidation.Results;

namespace FitTracker.Domain.Shared.ValidationErrors
{
    public class BadRequestError(IEnumerable<ValidationFailure> failures) : ValidationResult(failures);
}
