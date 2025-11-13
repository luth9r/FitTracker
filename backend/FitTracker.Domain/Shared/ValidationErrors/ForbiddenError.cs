using FluentValidation.Results;

namespace FitTracker.Domain.Shared.ValidationErrors
{
    public class ForbiddenError(IEnumerable<ValidationFailure> failures) : ValidationResult(failures);
}
