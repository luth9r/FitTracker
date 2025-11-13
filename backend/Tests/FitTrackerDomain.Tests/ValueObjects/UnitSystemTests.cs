using FitTracker.Domain.ValueObjects;
using FluentAssertions;

namespace FitTrackerDomain.Tests.ValueObjects
{
    public class UnitSystemTests
    {
        [Fact]
        public void Predefined_Units_Should_Have_Correct_Properties()
        {
            UnitSystem.Metric.Name.Should().Be("metric");
            UnitSystem.Metric.WeightUnit.Should().Be("kg");
            UnitSystem.Metric.LengthUnit.Should().Be("cm");
            UnitSystem.Metric.DistanceUnit.Should().Be("km");

            UnitSystem.Imperial.Name.Should().Be("imperial");
            UnitSystem.Imperial.WeightUnit.Should().Be("lbs");
            UnitSystem.Imperial.LengthUnit.Should().Be("in");
            UnitSystem.Imperial.DistanceUnit.Should().Be("mi");
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
                unitSystem.Should().NotBeNull();
                unitSystem.Name.Should().Be(value.ToLower());
            }
            else
            {
                Action act = () => UnitSystem.FromString(value);
                act.Should().Throw<ArgumentException>().WithMessage($"Invalid unit system: {value}");
            }
        }

        [Fact]
        public void GetAll_Should_Return_Both_Metric_And_Imperial()
        {
            var all = UnitSystem.GetAll().ToList();

            all.Should().Contain(UnitSystem.Metric);
            all.Should().Contain(UnitSystem.Imperial);
            all.Should().HaveCount(2);
        }

        [Fact]
        public void Equality_Should_Work_Correctly()
        {
            UnitSystem.Metric.Equals(UnitSystem.Metric).Should().BeTrue();
            UnitSystem.Metric.Equals(UnitSystem.Imperial).Should().BeFalse();
            UnitSystem.Metric.Should().Be(UnitSystem.FromString("metric"));
            (UnitSystem.Metric == UnitSystem.FromString("metric")).Should().BeTrue();
            (UnitSystem.Metric != UnitSystem.Imperial).Should().BeTrue();
        }

        [Theory]
        [InlineData(100, "metric", "imperial", 39.3701)]
        [InlineData(39.3701, "imperial", "metric", 100)]
        public void ConvertLength_Should_Convert_Correctly(decimal value, string from, string to, decimal expected)
        {
            var fromUnit = UnitSystem.FromString(from);
            var toUnit = UnitSystem.FromString(to);

            var convertedLength = fromUnit.ConvertLength(value, toUnit);

            convertedLength.Should().BeApproximately(expected, 0.0001m);
        }

        [Theory]
        [InlineData(10, "metric", "imperial", 22.0462)]
        [InlineData(22.0462, "imperial", "metric", 10)]
        public void ConvertWeight_Should_Convert_Correctly(decimal value, string from, string to, decimal expected)
        {
            var fromUnit = UnitSystem.FromString(from);
            var toUnit = UnitSystem.FromString(to);

            var convertedWeight = fromUnit.ConvertWeight(value, toUnit);

            convertedWeight.Should().BeApproximately(expected, 0.0001m);
        }
    }
}
