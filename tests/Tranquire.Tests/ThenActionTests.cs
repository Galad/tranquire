using AutoFixture.Idioms;
using FluentAssertions;
using Xunit;

namespace Tranquire.Tests
{
    public class ThenActionTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ThenAction<object, string>));
        }

        [Theory, DomainAutoData]
        public void Sut_ConstructorInitializedMember(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify<ThenAction<object, string>>(a => new object[]
            {
                a.VerifyAction
            });
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            ThenAction<object, string> sut)
        {
            // arrange
            // act
            var actual = sut.Name;
            // assert
            actual.Should().EndWith(sut.Question.Name);
        }
    }
}
