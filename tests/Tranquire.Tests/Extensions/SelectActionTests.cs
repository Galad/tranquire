using System;
using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{
    public class SelectActionTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(SelectAction<object, string>));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(SelectAction<object, string>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldReturnCorrectValue(
            object expected,
            string actionResult,
            IActor actor,
            IFixture fixture)
        {
            // arrange
            var mockFunc = new Mock<Func<string, object>>();
            fixture.Inject(mockFunc.Object);
            var sut = fixture.Create<SelectAction<string, object>>();
            mockFunc.Setup(s => s(actionResult)).Returns(expected);
            Mock.Get(actor).Setup(a => a.Execute(sut.Action)).Returns(actionResult);
            // act            
            var actual = sut.ExecuteGivenAs(actor);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldReturnCorrectValue(
            object expected,
            string actionResult,
            IActor actor,
            IFixture fixture)
        {
            // arrange
            var mockFunc = new Mock<Func<string, object>>();
            fixture.Inject(mockFunc.Object);
            var sut = fixture.Create<SelectAction<string, object>>();
            mockFunc.Setup(s => s(actionResult)).Returns(expected);
            Mock.Get(actor).Setup(a => a.Execute(sut.Action)).Returns(actionResult);
            // act            
            var actual = sut.ExecuteWhenAs(actor);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            SelectAction<string, object> sut,
            string expectedName)
        {
            // arrange
            Mock.Get(sut.Action).Setup(q => q.Name).Returns(expectedName);
            // act            
            var actual = sut.Name;
            // assert
            var expected = "[Select] " + expectedName;
            actual.Should().Be(expected);
        }
    }
}
