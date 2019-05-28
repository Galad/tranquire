using FluentAssertions;
using Moq;
using AutoFixture;
using AutoFixture.Idioms;
using System;
using Tranquire.Extensions;
using Xunit;
using System.Threading.Tasks;

namespace Tranquire.Tests.Extensions
{
    public class SelectQuestionAsyncTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(SelectQuestionAsync<object, string>));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(SelectQuestionAsync<object, string>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public async Task AnsweredBy_ShouldReturnCorrectValue(
            object expected,
            string actionResult,
            IActor actor,
            IFixture fixture)
        {
            // arrange
            var mockFunc = new Mock<Func<string, Task<object>>>();
            fixture.Inject(mockFunc.Object);
            var sut = fixture.Create<SelectQuestionAsync<string, object>>();
            mockFunc.Setup(s => s(actionResult)).Returns(Task.FromResult(expected));
            Mock.Get(actor).Setup(a => a.AsksFor(sut.Question)).Returns(Task.FromResult(actionResult));
            // act            
            var actual = await sut.AnsweredBy(actor);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            SelectQuestionAsync<string, object> sut,
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
