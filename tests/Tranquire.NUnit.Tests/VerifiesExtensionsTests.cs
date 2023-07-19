using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Tranquire;
using Tranquire.Tests;
using Xunit;
using NUnitF = NUnit.Framework;

namespace Tranquire.NUnit.Tests;

[Trait("Category", "")]
public class VerifiesExtensionsTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VerifiesExtensions));
    }

    private static IQuestion<T> Question<T>(T value) => Questions.Create("Question", _ => value);
    private static IQuestion<Task<T>> QuestionAsync<T>(T value) => Question(Task.FromResult(value));

    #region Synchronous
    [Theory, DomainAutoData]
    public void Then_WithConstraints_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        object expected)
    {
        // arrange
        var question = Question(expected);
        var constraint = NUnitF.Is.EqualTo(expected);
        // act                        
        var actual = actor.Then(question, constraint);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void Then_WithConstraints_WhenFailing_ShouldThrow(
        [Modest] Actor actor)
    {
        // arrange
        var question = Question(new object());
        var constraint = NUnitF.Is.EqualTo(new object());
        // act and assert
        Assert.Throws<NUnitF.AssertionException>(() => actor.Then(question, constraint));
    }

    [Theory, DomainAutoData]
    public void Then_WithConstraints_AndFuncExceptionMessage_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        object expected,
        string message)
    {
        // arrange
        var question = Question(expected);
        var constraint = NUnitF.Is.EqualTo(expected);
        // act                        
        var actual = actor.Then(question, constraint, () => message);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void Then_WithConstraints_AndFuncExceptionMessage_WhenFailing_ShouldThrow(
        [Modest] Actor actor,
        string message)
    {
        // arrange
        var question = Question(new object());
        var constraint = NUnitF.Is.EqualTo(new object());
        // act and assert
        new System.Action(() => actor.Then(question, constraint, () => message))
            .Should().Throw<NUnitF.AssertionException>()
            .Where(ex => ex.Message.Contains(message));
    }

    [Theory, DomainAutoData]
    public void Then_WithConstraints_AndExceptionMessage_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        object expected,
        string message,
        object[] args)
    {
        // arrange
        var question = Question(expected);
        var constraint = NUnitF.Is.EqualTo(expected);
        // act                        
        var actual = actor.Then(question, constraint, message, args);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void Then_WithConstraints_AndExceptionMessage_WhenFailing_ShouldThrow(
        [Modest] Actor actor,
        string[] args)
    {
        // arrange
        var message = string.Join("", args.Select((_, i) => $"{{{i}}}"));
        var question = Question(new object());
        var constraint = NUnitF.Is.EqualTo(new object());
        // act and assert
        var expectedMessage = string.Format(message, args);
        new System.Action(() => actor.Then(question, constraint, message, args))
            .Should().Throw<NUnitF.AssertionException>()
            .Where(ex => ex.Message.Contains(expectedMessage));
    }
    #endregion

    #region Async
    [Theory, DomainAutoData]
    public async Task Then_Async_WithConstraints_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        object expected)
    {
        // arrange
        var question = QuestionAsync(expected);
        var constraint = NUnitF.Is.EqualTo(expected);
        // act                        
        var actual = await actor.Then(question, constraint);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public async Task Then_Async_WithConstraints_WhenFailing_ShouldThrow(
        [Modest] Actor actor)
    {
        // arrange
        var question = QuestionAsync(new object());
        var constraint = NUnitF.Is.EqualTo(new object());
        // act and assert
        await Assert.ThrowsAsync<NUnitF.AssertionException>(() => actor.Then(question, constraint));
    }

    [Theory, DomainAutoData]
    public async Task Then_Async_WithConstraints_AndFuncExceptionMessage_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        object expected,
        string message)
    {
        // arrange
        var question = QuestionAsync(expected);
        var constraint = NUnitF.Is.EqualTo(expected);
        // act                        
        var actual = await actor.Then(question, constraint, () => message);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public async Task Then_Async_WithConstraints_AndFuncExceptionMessage_WhenFailing_ShouldThrow(
        [Modest] Actor actor,
        string message)
    {
        // arrange
        var question = QuestionAsync(new object());
        var constraint = NUnitF.Is.EqualTo(new object());
        // act and assert
        var ex = await Assert.ThrowsAsync<NUnitF.AssertionException>(() => actor.Then(question, constraint, () => message));
        Assert.Contains(message, ex.Message);
    }

    [Theory, DomainAutoData]
    public async Task Then_Async_WithConstraints_AndExceptionMessage_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        object expected,
        string message,
        object[] args)
    {
        // arrange
        var question = QuestionAsync(expected);
        var constraint = NUnitF.Is.EqualTo(expected);
        // act                        
        var actual = await actor.Then(question, constraint, message, args);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public async Task Then_Async_WithConstraints_AndExceptionMessage_WhenFailing_ShouldThrow(
        [Modest] Actor actor,
        string[] args)
    {
        // arrange
        var message = string.Join("", args.Select((_, i) => $"{{{i}}}"));
        var question = QuestionAsync(new object());
        var constraint = NUnitF.Is.EqualTo(new object());
        // act and assert
        var expectedMessage = string.Format(message, args);
        var ex = await Assert.ThrowsAsync<NUnitF.AssertionException>(() => actor.Then(question, constraint, message, args));
        Assert.Contains(expectedMessage, ex.Message);
    }
    #endregion
}
