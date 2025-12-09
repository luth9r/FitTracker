namespace FitTracker.Domain.ValueObjects
{
    /// <summary>
    /// ValueObject for unit system.
    /// </summary>
    public sealed class UnitSystem : IEquatable<UnitSystem>
    {
        public static readonly UnitSystem Metric = new("metric", "kg", "cm", "km");
        public static readonly UnitSystem Imperial = new("imperial", "lbs", "in", "mi");

        public string Name { get; }

        public string WeightUnit { get; }

        public string LengthUnit { get; }

        public string DistanceUnit { get; }

        private UnitSystem(string name, string weightUnit, string lengthUnit, string distanceUnit)
        {
            Name = name;
            WeightUnit = weightUnit;
            LengthUnit = lengthUnit;
            DistanceUnit = distanceUnit;
        }

        private UnitSystem()
        {
        }

        public static UnitSystem FromString(string value)
        {
            return value?.ToLower() switch
            {
                "metric" => Metric,
                "imperial" => Imperial,
                _ => throw new ArgumentException($"Invalid unit system: {value}")
            };
        }

        public static IEnumerable<UnitSystem> GetAll()
        {
            return new[] { Metric, Imperial };
        }

        public bool Equals(UnitSystem? other)
        {
            if (other is null)
            {
                return false;
            }

            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is UnitSystem other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(UnitSystem? left, UnitSystem? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UnitSystem? left, UnitSystem? right)
        {
            return !Equals(left, right);
        }

        public override string ToString() => Name;

        public decimal ConvertWeight(decimal weight, UnitSystem targetSystem)
        {
            if (this == targetSystem)
            {
                return weight;
            }

            // kg to lbs: multiply by 2.20462
            if (this == Metric && targetSystem == Imperial)
            {
                return weight * 2.20462m;
            }

            // lbs to kg: divide by 2.20462
            if (this == Imperial && targetSystem == Metric)
            {
                return weight / 2.20462m;
            }

            return weight;
        }

        public static decimal ConvertFromMetric(decimal weightInMetric, UnitSystem targetSystem)
        {
            return Metric.ConvertWeight(weightInMetric, targetSystem);
        }

        public decimal ConvertLength(decimal length, UnitSystem targetSystem)
        {
            if (this == targetSystem)
            {
                return length;
            }

            // cm to inches: divide by 2.54
            if (this == Metric && targetSystem == Imperial)
            {
                return length / 2.54m;
            }

            // inches to cm: multiply by 2.54
            if (this == Imperial && targetSystem == Metric)
            {
                return length * 2.54m;
            }

            return length;
        }
    }
}
