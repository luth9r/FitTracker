using FluentValidation.Results;

namespace FitTracker.Domain.Shared.ValidationErrors
{
    /// <summary>
    /// Represents a "Not Found" validation error.
    /// </summary>
    public class NotFoundError(IEnumerable<ValidationFailure> failures) : ValidationResult(failures);
}
