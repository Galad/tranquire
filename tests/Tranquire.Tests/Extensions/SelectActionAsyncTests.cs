using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions;

public class SelectActionAsyncTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SelectActionAsync<object, string>));
    }

    [Theory, DomainAutoData]
    public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
    {
        assertion.Verify(typeof(SelectActionAsync<object, string>).GetConstructors());
    }

    [Theory, DomainAutoData]
    public async Task ExecuteGivenAs_ShouldReturnCorrectValue(
        object expected,
        string actionResult,
        IActor actor,
        IFixture fixture)
    {
        // arrange
        var mockFunc = new Mock<Func<string, Task<object>>>();
        fixture.Inject(mockFunc.Object);
        var sut = fixture.Create<SelectActionAsync<string, object>>();
        mockFunc.Setup(s => s(actionResult)).Returns(Task.FromResult(expected));
        Mock.Get(actor).Setup(a => a.Execute(sut.Action)).Returns(Task.FromResult(actionResult));
        // act            
        var actual = await sut.ExecuteGivenAs(actor);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public async Task ExecuteWhenAs_ShouldReturnCorrectValue(
        object expected,
        string actionResult,
        IActor actor,
        IFixture fixture)
    {
        // arrange
        var mockFunc = new Mock<Func<string, Task<object>>>();
        fixture.Inject(mockFunc.Object);
        var sut = fixture.Create<SelectActionAsync<string, object>>();
        mockFunc.Setup(s => s(actionResult)).Returns(Task.FromResult(expected));
        Mock.Get(actor).Setup(a => a.Execute(sut.Action)).Returns(Task.FromResult(actionResult));
        // act            
        var actual = await sut.ExecuteWhenAs(actor);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void Name_ShouldReturnCorrectValue(
        SelectActionAsync<string, object> sut,
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
