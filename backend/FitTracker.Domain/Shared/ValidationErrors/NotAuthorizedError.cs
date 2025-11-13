using FluentValidation.Results;

namespace FitTracker.Domain.Shared.ValidationErrors
{
    public class NotAuthorizedError(IEnumerable<ValidationFailure> failures) : ValidationResult(failures);
}
