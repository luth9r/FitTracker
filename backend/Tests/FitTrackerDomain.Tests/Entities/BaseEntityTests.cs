using FitTracker.Domain.Entities;
using FluentAssertions;
using FluentValidation;

namespace FitTracker.Tests.Domain.Entities
{
    public class TestEntity : BaseEntity
    {
        protected override IValidator GetValidator()
        {
            return new InlineValidator<TestEntity>
            {
                v => v.RuleFor(x => x.Id).NotEmpty()
            };
        }
    }

    public class BaseEntityTests
    {
        [Fact]
        public void Constructor_Should_Generate_NewId_And_SetDates()
        {
            var entity = new TestEntity();

            entity.Id.Should().NotBe(Guid.Empty);
            entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            entity.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void SetDatabaseFields_Should_Set_Properties_Correctly()
        {
            var entity = new TestEntity();
            var id = Guid.NewGuid();
            var createdAt = DateTime.UtcNow.AddDays(-1);
            var updatedAt = DateTime.UtcNow;

            entity.SetDatabaseFields(id, createdAt, updatedAt);

            entity.Id.Should().Be(id);
            entity.CreatedAt.Should().Be(createdAt);
            entity.UpdatedAt.Should().Be(updatedAt);
        }

        [Fact]
        public void Validate_Should_Return_Valid_Result_For_Valid_Entity()
        {
            var entity = new TestEntity();

            var result = entity.Validate();

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_Return_True_For_Same_Instance()
        {
            var entity = new TestEntity();

            entity.Equals(entity).Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_Return_True_For_Different_Instances_With_Same_Type_And_Id()
        {
            var id = Guid.NewGuid();
            var entity1 = new TestEntity();
            var entity2 = new TestEntity();

            entity1.SetDatabaseFields(id, DateTime.UtcNow, DateTime.UtcNow);
            entity2.SetDatabaseFields(id, DateTime.UtcNow, DateTime.UtcNow);

            entity1.Equals(entity2).Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_Return_False_For_Different_Types()
        {
            var entity = new TestEntity();
            var other = new DummyEntity();

            entity.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_Should_Be_Consistent_For_Same_Id_And_Type()
        {
            var id = Guid.NewGuid();
            var entity1 = new TestEntity();
            var entity2 = new TestEntity();

            entity1.SetDatabaseFields(id, DateTime.UtcNow, DateTime.UtcNow);
            entity2.SetDatabaseFields(id, DateTime.UtcNow, DateTime.UtcNow);

            entity1.GetHashCode().Should().Be(entity2.GetHashCode());
        }

        private class DummyEntity : BaseEntity
        {
            protected override IValidator GetValidator() => throw new NotImplementedException();
        }
    }
}
