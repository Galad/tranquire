using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using System;
using Tranquire;
using Xunit;

namespace Tranquire.Tests
{
    public class ThenActionTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ThenAction<object>));
        }

        [Theory, DomainAutoData]
        public void Sut_ConstructorInitializedMember(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify<ThenAction<object>>(a => new object[]
            {
                a.Question,
                a.VerifyAction
            });
        }
        
        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            ThenAction<object> sut)
        {
            // arrange
            // act
            var actual = sut.Name;
            // assert
            actual.Should().EndWith(sut.Question.Name);
        }
    }
}
