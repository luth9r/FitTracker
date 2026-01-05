using CSharpFunctionalExtensions;
using FluentValidation.Results;

namespace FitTracker.Application.Extensions;

/// <summary>
///     Extension methods for <see cref="Result" />.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Creates a validation failure result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="propertyName">The name of the property that failed validation.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A <see cref="Result" /> containing the validation failure.</returns>
    public static Result<TValue, ValidationResult> ValidationFailure<TValue>(
        string propertyName,
        string errorMessage)
    {
        var errors = new ValidationResult(
            new[]
            {
                new ValidationFailure(propertyName, errorMessage),
            });
        return Result.Failure<TValue, ValidationResult>(errors);
    }

    /// <summary>
    ///     Creates a validation failure result from existing validation failures.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="propertyName">The name of the property (unused).</param>
    /// <param name="failures">The collection of <see cref="ValidationFailure" />.</param>
    /// <returns>A <see cref="Result" /> containing the validation failures.</returns>
    public static Result<TValue, ValidationResult> ValidationFailure<TValue>(
        string propertyName,
        IEnumerable<ValidationFailure> failures)
    {
        var validationResult = new ValidationResult(failures);
        return Result.Failure<TValue, ValidationResult>(validationResult);
    }

    /// <summary>
    ///     Creates a validation failure result from a collection of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">The list of errors containing property names and error messages.</param>
    /// <returns>A <see cref="Result" /> containing the validation failures.</returns>
    public static Result<TValue, ValidationResult> CreateValidationFailures<TValue>(
        params (string PropertyName, string ErrorMessage)[] errors)
    {
        var failures = errors.Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage));
        var validationResult = new ValidationResult(failures);

        return Result.Failure<TValue, ValidationResult>(validationResult);
    }
}
