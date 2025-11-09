// FitTracker.Domain/ValueObjects/Weight.cs
using System;

namespace FitTracker.Domain.ValueObjects
{
    /// <summary>
    /// Represents a weight value with automatic conversion between kilograms and pounds.
    /// Weight is always stored in kilograms in the database.
    /// </summary>
    public sealed class Weight : IEquatable<Weight>, IComparable<Weight>
    {
        private const decimal KgToLbsMultiplier = 2.20462m;
        private const decimal LbsToKgMultiplier = 0.453592m;
        private const decimal ComparisonTolerance = 0.01m;

        public const string KilogramUnit = "kg";
        public const string PoundUnit = "lbs";

        /// <summary>
        /// Gets the weight value in kilograms (always stored in kg in database)
        /// </summary>
        public decimal ValueInKg { get; }

        private Weight() { }

        private Weight(decimal valueInKg)
        {
            if (valueInKg < 0)
                throw new ArgumentException("Weight cannot be negative", nameof(valueInKg));

            ValueInKg = Math.Round(valueInKg, 2);
        }

        /// <summary>
        /// Creates a weight from kilograms
        /// </summary>
        /// <param name="kilograms">Weight value in kilograms</param>
        /// <returns><see cref="Weight"/> instance with value stored in kilograms</returns>
        public static Weight FromKilograms(decimal kilograms)
        {
            return new Weight(kilograms);
        }

        /// <summary>
        /// Creates a weight from pounds and converts it to kilograms for storage
        /// </summary>
        /// <param name="pounds">Weight value in pounds</param>
        /// <returns><see cref="Weight"/> instance with value converted to and stored in kilograms</returns>
        public static Weight FromPounds(decimal pounds)
        {
            if (pounds < 0)
                throw new ArgumentException("Weight cannot be negative", nameof(pounds));

            return new Weight(pounds * LbsToKgMultiplier);
        }

        /// <summary>
        /// Gets the weight value in kilograms
        /// </summary>
        /// <returns><see cref="Weight"/> in kilograms</returns>
        public decimal ToKilograms() => ValueInKg;

        /// <summary>
        /// Converts and returns the weight value in pounds
        /// </summary>
        /// <returns><see cref="Weight"/> converted to pounds</returns>
        public decimal ToPounds() => Math.Round(ValueInKg * KgToLbsMultiplier, 2);

        /// <summary>
        /// Adds two weights together
        /// </summary>
        /// <param name="other">Weight to add</param>
        /// <returns>New <see cref="Weight"/> instance with combined value in kilograms</returns>
        /// <exception cref="ArgumentNullException">Thrown when other is null</exception>
        public Weight Add(Weight other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return new Weight(ValueInKg + other.ValueInKg);
        }

        /// <summary>
        /// Subtracts one weight from another
        /// </summary>
        /// <param name="other">Weight to subtract</param>
        /// <returns>New <see cref="Weight"/> instance with difference in kilograms</returns>
        /// <exception cref="ArgumentNullException">Thrown when other is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when result would be negative</exception>
        public Weight Subtract(Weight other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            var result = ValueInKg - other.ValueInKg;
            if (result < 0)
                throw new InvalidOperationException("Result cannot be negative");

            return new Weight(result);
        }

        /// <summary>
        /// Multiplies weight by a scalar value
        /// </summary>
        /// <param name="multiplier">Multiplier value</param>
        /// <returns>New <see cref="Weight"/> instance with multiplied value in kilograms</returns>
        /// <exception cref="ArgumentException">Thrown when multiplier is negative</exception>
        public Weight Multiply(decimal multiplier)
        {
            if (multiplier < 0)
                throw new ArgumentException("Multiplier cannot be negative", nameof(multiplier));

            return new Weight(ValueInKg * multiplier);
        }

        public static Weight operator +(Weight left, Weight right) => left.Add(right);
        public static Weight operator -(Weight left, Weight right) => left.Subtract(right);
        public static Weight operator *(Weight weight, decimal multiplier) => weight.Multiply(multiplier);
        public static bool operator >(Weight left, Weight right) => left.CompareTo(right) > 0;
        public static bool operator <(Weight left, Weight right) => left.CompareTo(right) < 0;
        public static bool operator >=(Weight left, Weight right) => left.CompareTo(right) >= 0;
        public static bool operator <=(Weight left, Weight right) => left.CompareTo(right) <= 0;

        public bool Equals(Weight? other)
        {
            if (other == null) return false;
            return Math.Abs(ValueInKg - other.ValueInKg) < ComparisonTolerance;
        }

        public int CompareTo(Weight? other)
        {
            if (other == null) return 1;
            return ValueInKg.CompareTo(other.ValueInKg);
        }

        public override bool Equals(object? obj) => obj is Weight weight && Equals(weight);
        public override int GetHashCode() => ValueInKg.GetHashCode();
        public static bool operator ==(Weight? left, Weight? right) => Equals(left, right);
        public static bool operator !=(Weight? left, Weight? right) => !Equals(left, right);

        /// <summary>
        /// Returns string representation in kilograms
        /// </summary>
        /// <returns>String in format "75.50 kg"</returns>
        public override string ToString() => $"{ValueInKg:F2} kg";

        /// <summary>
        /// Returns string representation in specified unit
        /// </summary>
        /// <param name="unit">Target unit ("kg" or "lbs")</param>
        /// <returns>String in format "75.50 kg" or "166.45 lbs"</returns>
        public string ToString(string unit)
        {
            return unit switch
            {
                KilogramUnit => $"{ValueInKg:F2} kg",
                PoundUnit => $"{ToPounds():F2} lbs",
                _ => ToString()
            };
        }
    }
}
