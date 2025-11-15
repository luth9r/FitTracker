using CSharpFunctionalExtensions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.Extensions
{
    public static class ResultExtensions
    {
        public static Result<TValue, ValidationResult> ValidationFailure<TValue>(
            string propertyName,
            string errorMessage)
        {
            var errors = new ValidationResult(new[]
            {
                new ValidationFailure(propertyName, errorMessage)
            });
            return Result.Failure<TValue, ValidationResult>(errors);
        }

        public static Result<TValue, ValidationResult> CreateValidationFailures<TValue>(params (string PropertyName, string ErrorMessage)[] errors)
        {
            var failures = errors.Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage));
            var validationResult = new ValidationResult(failures);

            return Result.Failure<TValue, ValidationResult>(validationResult);
        }

        public static Result<TValue, ValidationResult> ValidationFailure<TValue>(
            string propertyName,
            IEnumerable<ValidationFailure> failures)
        {
            var validationResult = new ValidationResult(failures);
            return Result.Failure<TValue, ValidationResult>(validationResult);
        }
    }
}
