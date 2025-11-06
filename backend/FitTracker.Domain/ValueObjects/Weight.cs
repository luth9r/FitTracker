using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Domain.ValueObjects
{
    public class Weight : IEquatable<Weight>, IComparable<Weight>
    {
        public decimal Value { get; }
        public string Unit { get; } // kg, lbs

        private Weight(decimal value, string unit)
        {
            if (value <= 0)
                throw new ArgumentException("Weight must be greater than 0");

            Value = value;
            Unit = unit;
        }

        public static Weight CreateKg(decimal kilograms)
        {
            return new Weight(kilograms, "kg");
        }

        public static Weight CreateLbs(decimal pounds)
        {
            return new Weight(pounds, "lbs");
        }

        public decimal ToKilograms()
        {
            return Unit == "kg" ? Value : Value * 0.453592m;
        }

        public decimal ToPounds()
        {
            return Unit == "lbs" ? Value : Value * 2.20462m;
        }

        public override bool Equals(object obj)
        {
            return obj is Weight weight && Equals(weight);
        }

        public bool Equals(Weight other)
        {
            if (other == null) return false;
            return Math.Abs(ToKilograms() - other.ToKilograms()) < 0.01m;
        }

        public int CompareTo(Weight other)
        {
            if (other == null) return 1;
            return ToKilograms().CompareTo(other.ToKilograms());
        }

        public override int GetHashCode()
        {
            return ToKilograms().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}
