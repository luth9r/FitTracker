using FitTracker.Domain.ValueObjects;
using FluentAssertions;

namespace FitTracker.Tests.Domain.ValueObjects
{
    public class WeightTests
    {
        [Fact]
        public void FromKilograms_Should_Create_Weight_With_Correct_Value()
        {
            var weight = Weight.FromKilograms(50m);

            _ = weight.ValueInKg.Should().Be(50m);
            _ = weight.ToKilograms().Should().Be(50m);
        }

        [Fact]
        public void FromKilograms_Should_Round_To_Two_Decimal_Places()
        {
            var weight = Weight.FromKilograms(50.12345m);

            _ = weight.ValueInKg.Should().Be(50.12m);
        }

        [Fact]
        public void FromKilograms_Should_Throw_When_Negative()
        {
            Action act = () => Weight.FromKilograms(-10m);

            _ = act.Should().Throw<ArgumentException>()
               .WithMessage("Weight cannot be negative*");
        }

        [Fact]
        public void FromPounds_Should_Create_Weight_And_Convert_To_Kilograms()
        {
            var weight = Weight.FromPounds(220.462m);

            _ = weight.ValueInKg.Should().BeApproximately(100m, 0.01m);
        }

        [Fact]
        public void FromPounds_Should_Throw_When_Negative()
        {
            Action act = () => Weight.FromPounds(-10m);

            _ = act.Should().Throw<ArgumentException>()
               .WithMessage("Weight cannot be negative*");
        }

        [Fact]
        public void ToKilograms_Should_Return_Value_In_Kg()
        {
            var weight = Weight.FromKilograms(75m);

            _ = weight.ToKilograms().Should().Be(75m);
        }

        [Fact]
        public void ToPounds_Should_Convert_Kg_To_Lbs()
        {
            var weight = Weight.FromKilograms(100m);

            _ = weight.ToPounds().Should().BeApproximately(220.46m, 0.01m);
        }

        [Fact]
        public void ToPounds_Should_Round_To_Two_Decimal_Places()
        {
            var weight = Weight.FromKilograms(45.5m);
            var pounds = weight.ToPounds();

            _ = Math.Round(pounds, 2).Should().Be(pounds);
        }

        [Fact]
        public void Add_Should_Combine_Two_Weights()
        {
            var weight1 = Weight.FromKilograms(50m);
            var weight2 = Weight.FromKilograms(25m);

            var result = weight1.Add(weight2);

            _ = result.ValueInKg.Should().Be(75m);
        }

        [Fact]
        public void Add_Should_Throw_When_Other_Is_Null()
        {
            var weight = Weight.FromKilograms(50m);

            Action act = () => weight.Add(null);

            _ = act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Subtract_Should_Calculate_Difference()
        {
            var weight1 = Weight.FromKilograms(100m);
            var weight2 = Weight.FromKilograms(30m);

            var result = weight1.Subtract(weight2);

            _ = result.ValueInKg.Should().Be(70m);
        }

        [Fact]
        public void Subtract_Should_Throw_When_Result_Would_Be_Negative()
        {
            var weight1 = Weight.FromKilograms(30m);
            var weight2 = Weight.FromKilograms(50m);

            Action act = () => weight1.Subtract(weight2);

            _ = act.Should().Throw<InvalidOperationException>()
               .WithMessage("Result cannot be negative");
        }

        [Fact]
        public void Subtract_Should_Throw_When_Other_Is_Null()
        {
            var weight = Weight.FromKilograms(50m);

            Action act = () => weight.Subtract(null);

            _ = act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Multiply_Should_Multiply_Weight_By_Scalar()
        {
            var weight = Weight.FromKilograms(25m);

            var result = weight.Multiply(3m);

            _ = result.ValueInKg.Should().Be(75m);
        }

        [Fact]
        public void Multiply_Should_Throw_When_Multiplier_Is_Negative()
        {
            var weight = Weight.FromKilograms(25m);

            Action act = () => weight.Multiply(-2m);

            _ = act.Should().Throw<ArgumentException>()
               .WithMessage("Multiplier cannot be negative*");
        }

        [Fact]
        public void Operator_Plus_Should_Add_Weights()
        {
            var weight1 = Weight.FromKilograms(40m);
            var weight2 = Weight.FromKilograms(35m);

            var result = weight1 + weight2;

            _ = result.ValueInKg.Should().Be(75m);
        }

        [Fact]
        public void Operator_Minus_Should_Subtract_Weights()
        {
            var weight1 = Weight.FromKilograms(80m);
            var weight2 = Weight.FromKilograms(30m);

            var result = weight1 - weight2;

            _ = result.ValueInKg.Should().Be(50m);
        }

        [Fact]
        public void Operator_Multiply_Should_Multiply_Weight()
        {
            var weight = Weight.FromKilograms(20m);

            var result = weight * 2.5m;

            _ = result.ValueInKg.Should().Be(50m);
        }

        [Fact]
        public void Operator_GreaterThan_Should_Compare_Weights()
        {
            var weight1 = Weight.FromKilograms(100m);
            var weight2 = Weight.FromKilograms(50m);

            _ = (weight1 > weight2).Should().BeTrue();
            _ = (weight2 > weight1).Should().BeFalse();
        }

        [Fact]
        public void Operator_LessThan_Should_Compare_Weights()
        {
            var weight1 = Weight.FromKilograms(50m);
            var weight2 = Weight.FromKilograms(100m);

            _ = (weight1 < weight2).Should().BeTrue();
            _ = (weight2 < weight1).Should().BeFalse();
        }

        [Fact]
        public void Operator_GreaterThanOrEqual_Should_Compare_Weights()
        {
            var weight1 = Weight.FromKilograms(100m);
            var weight2 = Weight.FromKilograms(100m);
            var weight3 = Weight.FromKilograms(50m);

            _ = (weight1 >= weight2).Should().BeTrue();
            _ = (weight1 >= weight3).Should().BeTrue();
            _ = (weight3 >= weight1).Should().BeFalse();
        }

        [Fact]
        public void Operator_LessThanOrEqual_Should_Compare_Weights()
        {
            var weight1 = Weight.FromKilograms(50m);
            var weight2 = Weight.FromKilograms(50m);
            var weight3 = Weight.FromKilograms(100m);

            _ = (weight1 <= weight2).Should().BeTrue();
            _ = (weight1 <= weight3).Should().BeTrue();
            _ = (weight3 <= weight1).Should().BeFalse();
        }

        [Fact]
        public void Equals_Should_Return_True_For_Same_Values_Within_Tolerance()
        {
            var weight1 = Weight.FromKilograms(50.00m);
            var weight2 = Weight.FromKilograms(50.00m);

            _ = weight1.Equals(weight2).Should().BeTrue();
            _ = (weight1 == weight2).Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_Return_False_For_Different_Values()
        {
            var weight1 = Weight.FromKilograms(50m);
            var weight2 = Weight.FromKilograms(75m);

            _ = weight1.Equals(weight2).Should().BeFalse();
            _ = (weight1 == weight2).Should().BeFalse();
        }

        [Fact]
        public void Equals_Should_Return_False_When_Other_Is_Null()
        {
            var weight = Weight.FromKilograms(50m);

            _ = weight.Equals(null).Should().BeFalse();
            _ = (weight == null).Should().BeFalse();
        }

        [Fact]
        public void NotEquals_Operator_Should_Work_Correctly()
        {
            var weight1 = Weight.FromKilograms(50m);
            var weight2 = Weight.FromKilograms(75m);

            _ = (weight1 != weight2).Should().BeTrue();
        }

        [Fact]
        public void CompareTo_Should_Return_Correct_Comparison_Result()
        {
            var weight1 = Weight.FromKilograms(50m);
            var weight2 = Weight.FromKilograms(75m);
            var weight3 = Weight.FromKilograms(50m);

            _ = weight1.CompareTo(weight2).Should().BeLessThan(0);
            _ = weight2.CompareTo(weight1).Should().BeGreaterThan(0);
            _ = weight1.CompareTo(weight3).Should().Be(0);
        }

        [Fact]
        public void CompareTo_Should_Return_1_When_Other_Is_Null()
        {
            var weight = Weight.FromKilograms(50m);

            _ = weight.CompareTo(null).Should().Be(1);
        }

        [Fact]
        public void GetHashCode_Should_Return_Same_Value_For_Equal_Weights()
        {
            var weight1 = Weight.FromKilograms(50m);
            var weight2 = Weight.FromKilograms(50m);

            _ = weight1.GetHashCode().Should().Be(weight2.GetHashCode());
        }

        [Fact]
        public void ToString_Should_Return_Value_In_Kg_With_Two_Decimals()
        {
            var weight = Weight.FromKilograms(75.5m);

            _ = weight.ToString().Should().Be("75.50 kg");
        }

        [Fact]
        public void ToString_With_Kg_Unit_Should_Return_Value_In_Kg()
        {
            var weight = Weight.FromKilograms(80m);

            _ = weight.ToString("kg").Should().Be("80.00 kg");
        }

        [Fact]
        public void ToString_With_Lbs_Unit_Should_Return_Value_In_Lbs()
        {
            var weight = Weight.FromKilograms(100m);

            _ = weight.ToString("lbs").Should().Be("220.46 lbs");
        }

        [Fact]
        public void ToString_With_Unknown_Unit_Should_Return_Default_Format()
        {
            var weight = Weight.FromKilograms(50m);

            _ = weight.ToString("unknown").Should().Be("50.00 kg");
        }
    }
}
