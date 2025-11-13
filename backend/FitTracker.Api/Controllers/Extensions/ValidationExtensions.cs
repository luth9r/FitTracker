using FitTracker.Domain.Shared.ValidationErrors;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FitTracker.Api.Controllers.Extensions
{
    internal static class ValidationExtensions
    {
        /// <summary>
        /// Transform <see cref="ValidationResult"/> to <see cref="ModelStateDictionary"/>
        /// </summary>
        /// <param name="validationResult"></param>
        /// <returns></returns>
        public static ModelStateDictionary ToModelState(this ValidationResult validationResult)
        {
            var modelState = new ModelStateDictionary();

            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return modelState;
        }
    }
}
