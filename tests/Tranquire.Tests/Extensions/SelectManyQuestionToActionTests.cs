using FluentAssertions;
using Moq;
using AutoFixture;
using AutoFixture.Idioms;
using System;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{
    public class SelectManyQuestionToActionTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(SelectManyQuestionToAction<object, string>));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(SelectManyQuestionToAction<object, string>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void AnsweredBy_ShouldReturnCorrectValue(
            object expected,
            string actionResult,
            IFixture fixture)
        {
            // arrange
            var actor = new Actor("John");
            var mockFunc = new Mock<Func<string, IAction<object>>>();
            fixture.Inject(mockFunc.Object);
            fixture.Inject(Questions.FromResult(actionResult));
            var sut = fixture.Create<SelectManyQuestionToAction<string, object>>();
            mockFunc.Setup(s => s(actionResult)).Returns(Actions.FromResult(expected));
            // act            
            var actual = actor.When(sut);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            SelectManyQuestionToAction<string, object> sut,
            string expectedName)
        {
            // arrange
            Mock.Get(sut.Question).Setup(q => q.Name).Returns(expectedName);
            // act            
            var actual = sut.Name;
            // assert
            var expected = "[SelectMany] " + expectedName;
            actual.Should().Be(expected);
        }
    }
}
