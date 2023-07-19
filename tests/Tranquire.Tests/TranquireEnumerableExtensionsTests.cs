using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tranquire.Tests;

public class TranquireEnumerableExtensionsTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(TranquireEnumerableExtensions));
    }

    [Theory, DomainAutoData]
    public void ToAction_ShouldBeActionThatCallsAllActions(
        IAction<Unit>[] actions,
        string name,
        Mock<IActor> actor)
    {
        //arrange            
        //act
        var actual = actions.ToAction(name);
        actual.ExecuteWhenAs(actor.Object);
        //assert
        actual.Name.Should().Be(name);
        foreach (var action in actions)
        {
            actor.Verify(m => m.Execute(action));
        }
    }

    #region ToAction async
    [Theory, DomainAutoData]
    public async Task ToAction_Async_ShouldBeActionThatCallsAllActionsSequentially(
        string name)
    {
        //arrange
        var result = new List<int>();
        var actions = Enumerable.Range(0, 10)
                                .Select(i => Actions.Create($"action{i}", async _ =>
                                {
                                    result.Add(i);
                                    await Task.Delay(5);
                                    result.Add(i);
                                }));
        var actor = new Actor("John");
        //act
        var actual = actions.ToAction(name);
        await actor.When(actual);
        //assert
        actual.Name.Should().Be(name);
        var expected = Enumerable.Range(0, 10).SelectMany(i => new[] { i, i }).ToList();
        Assert.Equal(expected, result);
    }
    #endregion
}
