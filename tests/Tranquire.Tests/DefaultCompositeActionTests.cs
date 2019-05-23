using FluentAssertions;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using System.Collections.Immutable;
using Xunit;

namespace Tranquire.Tests
{
    public class DefaultCompositeActionTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DefaultCompositeAction));
        }

        [Theory, DomainAutoData]
        public void Sut_IsCompositeAction(DefaultCompositeAction sut)
        {
            sut.Should().BeAssignableTo<CompositeAction>();
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue([Frozen]string expected, DefaultCompositeAction sut)
        {
            // act
            var actual = sut.Name;
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Actions_ShouldReturnCorrectValue([Frozen]IAction<Unit>[] expected, DefaultCompositeAction sut)
        {
            // act
            var actual = sut.Actions;
            // assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
