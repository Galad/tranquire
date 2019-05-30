using System;
using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{
    public class SelectQuestionTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(SelectQuestion<object, string>));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(SelectQuestion<object, string>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void AnsweredBy_ShouldReturnCorrectValue(
            object expected,
            string actionResult,
            IActor actor,
            IFixture fixture)
        {
            // arrange
            var mockFunc = new Mock<Func<string, object>>();
            fixture.Inject(mockFunc.Object);
            var sut = fixture.Create<SelectQuestion<string, object>>();
            mockFunc.Setup(s => s(actionResult)).Returns(expected);
            Mock.Get(actor).Setup(a => a.AsksFor(sut.Question)).Returns(actionResult);
            // act            
            var actual = sut.AnsweredBy(actor);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            SelectQuestion<string, object> sut,
            string expectedName)
        {
            // arrange
            Mock.Get(sut.Question).Setup(q => q.Name).Returns(expectedName);
            // act            
            var actual = sut.Name;
            // assert
            var expected = "[Select] " + expectedName;
            actual.Should().Be(expected);
        }
    }
}
