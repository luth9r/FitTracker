using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Domain.Enums
{
    /// <summary>
    /// Defines which exercises should be returned by the query.
    /// </summary>
    public enum ExerciseFilterType
    {
        /// <summary>
        /// Returns both standard exercises and custom exercises that belong to the current user.
        /// </summary>
        All,

        /// <summary>
        /// Returns only standard (system-defined) exercises.
        /// </summary>
        Standard,

        /// <summary>
        /// Returns only custom exercises created by the current user.
        /// </summary>
        Custom,
    }
}
