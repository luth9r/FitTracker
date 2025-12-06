using FitTracker.Domain.ValueObjects;
using FluentAssertions;

namespace FitTrackerDomain.Tests.ValueObjects
{
    public class UnitSystemTests
    {
        [Fact]
        public void Predefined_Units_Should_Have_Correct_Properties()
        {
            _ = UnitSystem.Metric.Name.Should().Be("metric");
            _ = UnitSystem.Metric.WeightUnit.Should().Be("kg");
            _ = UnitSystem.Metric.LengthUnit.Should().Be("cm");
            _ = UnitSystem.Metric.DistanceUnit.Should().Be("km");

            _ = UnitSystem.Imperial.Name.Should().Be("imperial");
            _ = UnitSystem.Imperial.WeightUnit.Should().Be("lbs");
            _ = UnitSystem.Imperial.LengthUnit.Should().Be("in");
            _ = UnitSystem.Imperial.DistanceUnit.Should().Be("mi");
        }

        [Theory]
        [InlineData("metric", true)]
        [InlineData("imperial", true)]
        [InlineData("METRIC", true)]
        [InlineData("IMPERIAL", true)]
        [InlineData("unknown", false)]
        public void FromString_Should_Return_Instance_For_Valid_Values_Or_Throw(string value, bool isValid)
        {
            if (isValid)
            {
                var unitSystem = UnitSystem.FromString(value);
                _ = unitSystem.Should().NotBeNull();
                _ = unitSystem.Name.Should().Be(value.ToLower());
            }
            else
            {
                Action act = () => UnitSystem.FromString(value);
                _ = act.Should().Throw<ArgumentException>().WithMessage($"Invalid unit system: {value}");
            }
        }

        [Fact]
        public void GetAll_Should_Return_Both_Metric_And_Imperial()
        {
            var all = UnitSystem.GetAll().ToList();

            _ = all.Should().Contain(UnitSystem.Metric);
            _ = all.Should().Contain(UnitSystem.Imperial);
            _ = all.Should().HaveCount(2);
        }

        [Fact]
        public void Equality_Should_Work_Correctly()
        {
            _ = UnitSystem.Metric.Equals(UnitSystem.Metric).Should().BeTrue();
            _ = UnitSystem.Metric.Equals(UnitSystem.Imperial).Should().BeFalse();
            _ = UnitSystem.Metric.Should().Be(UnitSystem.FromString("metric"));
            _ = (UnitSystem.Metric == UnitSystem.FromString("metric")).Should().BeTrue();
            _ = (UnitSystem.Metric != UnitSystem.Imperial).Should().BeTrue();
        }

        [Theory]
        [InlineData(100, "metric", "imperial", 39.3701)]
        [InlineData(39.3701, "imperial", "metric", 100)]
        public void ConvertLength_Should_Convert_Correctly(decimal value, string from, string to, decimal expected)
        {
            var fromUnit = UnitSystem.FromString(from);
            var toUnit = UnitSystem.FromString(to);

            var convertedLength = fromUnit.ConvertLength(value, toUnit);

            _ = convertedLength.Should().BeApproximately(expected, 0.0001m);
        }

        [Theory]
        [InlineData(10, "metric", "imperial", 22.0462)]
        [InlineData(22.0462, "imperial", "metric", 10)]
        public void ConvertWeight_Should_Convert_Correctly(decimal value, string from, string to, decimal expected)
        {
            var fromUnit = UnitSystem.FromString(from);
            var toUnit = UnitSystem.FromString(to);

            var convertedWeight = fromUnit.ConvertWeight(value, toUnit);

            _ = convertedWeight.Should().BeApproximately(expected, 0.0001m);
        }
    }
}
