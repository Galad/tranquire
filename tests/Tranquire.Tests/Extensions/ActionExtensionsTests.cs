using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions;

public class ActionExtensionsTests
{
    public class Ability
    {
    }

    private static readonly IActorFacade TestActor = new Actor("Test").CanUse(new Ability());

    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion) => assertion.Verify(typeof(ActionExtensions));

    #region If
    [Fact]
    public void If_ShouldReturnCorrectValueFalse()
    {
        ExecuteAndAssertIf<object>((action, defaultValue, predicate) =>
            action.If(() => predicate, defaultValue));
    }

    [Fact]
    public void If_UnitFalse_ShouldReturnCorrectValue()
    {
        ExecuteAndAssertIf<Unit>((action, _, predicate) => action.If(() => predicate));
    }

    [Fact]
    public void If_PredicateAbilityFalse_ShouldReturnCorrectValue()
    {
        ExecuteAndAssertIf<object>((action, defaultValue, predicate) =>
            action.If((Ability _) => predicate, defaultValue));
    }

    [Fact]
    public void If_PredicateAbilityFalse_Unit_ShouldReturnCorrectValue_test()
    {
        ExecuteAndAssertIf<Unit>((action, _, predicate) => action.If((Ability _) => predicate));
    }

    [Fact]
    public void If_Question_ActionUnit_ShouldReturnCorrectValue()
    {
        ExecuteAndAssertIf<Unit>((action, _, predicate) =>
        {
            var question = Questions.Create("predicate", predicate);
            return action.If(question);
        });
    }

    [Fact]
    public void If_Question_Action_ShouldReturnCorrectValue()
    {
        ExecuteAndAssertIf<object>((action, defaultValue, predicate) =>
        {
            var question = Questions.Create("predicate", predicate);
            return action.If(question, defaultValue);
        });
    }

    private static void ExecuteAndAssertIf<T>(
        Func<IAction<T>, object, bool, IAction<T>> sutFactory)
    {
        var actualGivenTrue = Execute(true, TestActor.Given);
        var actualGivenFalse = Execute(false, TestActor.Given);
        var actualWhenTrue = Execute(true, TestActor.When);
        var actualWhenFalse = Execute(false, TestActor.When);
        
        using (new AssertionScope())
        {
            actualGivenTrue.result.IsExecuted.Should().BeTrue();
            actualGivenTrue.actual.Should().Be(default(T));
            actualGivenFalse.result.IsExecuted.Should().BeFalse();
            actualGivenFalse.actual.Should().Be(actualGivenFalse.result.DefaultValue);
            actualWhenTrue.result.IsExecuted.Should().BeTrue();
            actualWhenTrue.actual.Should().Be(default(T));
            actualWhenFalse.result.IsExecuted.Should().BeFalse();
            actualWhenFalse.actual.Should().Be(actualWhenFalse.result.DefaultValue);
        }

        (IfActionResult result, T actual) Execute(
            bool predicateValue,
            Func<IAction<T>, T> executeMethod)
        {
            
            var (result, action) = CreateAction<T>();
            var sut = sutFactory(action, result.DefaultValue, predicateValue);
            var actual = executeMethod(sut);
            return (result, actual);
        }
    }

    private class IfActionResult
    {
        public object Value { get; set; }

        public object DefaultValue { get; set; }

        public bool IsExecuted => Value?.ToString() == "updated";
    }

    private static (IfActionResult, IAction<T>) CreateAction<T>()
    {
        var result = new IfActionResult {
            DefaultValue = typeof(T) == typeof(Unit) ? Unit.Default : new object()
        };
        var action = Actions.Create("test",
            _ =>
            {
                result.Value = "updated";
                return default(T);
            });
        return (result, action);
    }

    #endregion

    #region AsActionUnit

    [Theory, DomainAutoData]
    public void AsActionUnit_ExecuteWhen_ShouldCallExecuteWhenOnSourceAction(Mock<IAction<object>> action, IActor actor)
    {
        //act
        var actual = ActionExtensions.AsActionUnit(action.Object);
        actual.ExecuteWhenAs(actor);
        //assert            
        action.Verify(a => a.ExecuteWhenAs(actor));
    }

    [Theory, DomainAutoData]
    public void AsActionUnit_ExecuteGiven_ShouldCallExecuteGivenOnSourceAction(Mock<IAction<object>> action,
        IActor actor)
    {
        //act
        var actual = ActionExtensions.AsActionUnit(action.Object);
        actual.ExecuteGivenAs(actor);
        //assert            
        action.Verify(a => a.ExecuteGivenAs(actor));
    }

    [Theory, DomainAutoData]
    public void AsActionUnit_ShouldReturnCorrectValue(Mock<IAction<object>> action)
    {
        //act
        var actual = ActionExtensions.AsActionUnit(action.Object);
        //assert
        actual.Should().BeAssignableTo<IAction<Unit>>();
        actual.Name.Should().Be(action.Object.Name);
    }

    #endregion

    #region Using

    [Theory, DomainAutoData]
    public void Using_ShouldReturnCorrectValue(IAction<IDisposable> disposableAction, IAction<object> action)
    {
        // act
        var actual = ActionExtensions.Using(action, disposableAction);
        // assert
        var expected = new UsingAction<object>(disposableAction, action);
        actual.Should().BeEquivalentTo(expected);
    }

    #endregion

    #region SelectMany: IAction<T> -> IAction<U>

    [Theory, DomainAutoData]
    public void SelectMany_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var action = Actions.FromResult(value1)
            .SelectMany(v => Actions.FromResult(v + value2));
        var actor = new Actor("john");
        // act
        var actual = actor.When(action);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_2_ShouldReturnCorrectResult(
        int value1,
        int value2,
        int value3)
    {
        // arrange
        var action = Actions.FromResult(value1)
            .SelectMany(v => Actions.FromResult(v + value2))
            .SelectMany(v => Actions.FromResult(v + value3));
        var actor = new Actor("john");
        // act
        var actual = actor.When(action);
        // assert
        var expected = value1 + value2 + value3;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Name_ShouldReturnCorrectResult(
        IAction<int> action1,
        IAction<int> action2)
    {
        // arrange
        var action = action1.SelectMany(_ => action2);
        // act
        var actual = action.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<T> -> IQuestion<U>

    [Theory, DomainAutoData]
    public void SelectMany_ReturningQuestion_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var question = Actions.FromResult(value1)
            .SelectMany(v => Questions.FromResult(v + value2));
        var actor = new Actor("john");
        // act
        var actual = actor.AsksFor(question);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_ReturningQuestion_Name_ShouldReturnCorrectResult(
        IAction<int> action,
        IQuestion<int> question1)
    {
        // arrange
        var question = action.SelectMany(_ => question1);
        // act
        var actual = question.Name;
        // assert
        var expected = "[SelectMany] " + action.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task<T>> -> IAction<U>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var action = Actions.FromResult(Task.FromResult(value1))
            .SelectMany(v => Actions.FromResult(v + value2));
        // act
        var actual = await actor.When(action);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public async Task SelectMany_Async2_ShouldReturnCorrectResult(
        int value1,
        int value2,
        int value3)
    {
        // arrange
        var actor = new Actor("John");
        var action = Actions.FromResult(Task.FromResult(value1))
            .SelectMany(v => Actions.FromResult(v + value2))
            .SelectMany(v => Actions.FromResult(v + value3));
        // act
        var actual = await actor.When(action);
        // assert
        var expected = value1 + value2 + value3;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Name_ShouldReturnCorrectResult(
        IAction<Task<int>> action1,
        IAction<int> action2)
    {
        // arrange
        var action = action1.SelectMany<int, int>(_ => action2);
        // act
        var actual = action.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task<T>> -> IAction<Task<U>>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_ReturningAsyncAction_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var action = Actions.FromResult(Task.FromResult(value1))
            .SelectMany(v => Actions.FromResult(Task.FromResult(v + value2)));
        // act
        var actual = await actor.When(action);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public async Task SelectMany_Async2_ReturningAsyncAction_ShouldReturnCorrectResult(
        int value1,
        int value2,
        int value3)
    {
        // arrange
        var actor = new Actor("John");
        var action = Actions.FromResult(Task.FromResult(value1))
            .SelectMany(v => Actions.FromResult(Task.FromResult(v + value2)))
            .SelectMany(v => Actions.FromResult(Task.FromResult(v + value3)));
        // act
        var actual = await actor.When(action);
        // assert
        var expected = value1 + value2 + value3;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Name_ReturningAsyncAction_ShouldReturnCorrectResult(
        IAction<Task<int>> action1,
        IAction<Task<int>> action2)
    {
        // arrange
        var action = action1.SelectMany<int, int>(_ => action2);
        // act
        var actual = action.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task<T>> -> IAction<Task>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_ReturningAsyncTaskAction_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var actual = 0;
        var action = Actions.FromResult(Task.FromResult(value1))
            .SelectMany(v => Actions.Create("set result", _ => Delay(() => { actual = v + value2; })));
        // act
        var task = actor.When(action);
        await task;
        // assert            
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Name_ReturningAsyncTaskAction_ShouldReturnCorrectResult(
        IAction<Task<int>> action1,
        IAction<Task> action2)
    {
        // arrange
        var action = action1.SelectMany<int>(_ => action2);
        // act
        var actual = action.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task<T>> -> IQuestion<T>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_ReturningQuestion_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var question = Actions.FromResult(Task.FromResult(value1))
            .SelectMany(v => Questions.FromResult(v + value2));
        // act
        var actual = await actor.AsksFor(question);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Name_ReturningQuestion_ShouldReturnCorrectResult(
        IAction<Task<int>> action1,
        IQuestion<int> question2)
    {
        // arrange
        var question = action1.SelectMany<int, int>(_ => question2);
        // act
        var actual = question.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task<T>> -> IQuestion<Task<T>>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_ReturningAsyncQuestion_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var question = Actions.FromResult(Task.FromResult(value1))
            .SelectMany(v => Questions.FromResult(Task.FromResult(v + value2)));
        // act
        var actual = await actor.AsksFor(question);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Name_ReturningAsyncQuestion_ShouldReturnCorrectResult(
        IAction<Task<int>> action1,
        IQuestion<Task<int>> question2)
    {
        // arrange
        var question = action1.SelectMany<int, int>(_ => question2);
        // act
        var actual = question.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task> -> IAction<U>

    [Theory, DomainAutoData]
    public async Task SelectMany_AsyncTask_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var currentValue = 0;
        var action = Actions.Create("set value", _ => Delay(() => currentValue = value1))
            .SelectMany(() => Actions.FromResult(currentValue + value2));
        // act
        var actual = await actor.When(action);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_AsyncTask_Name_ShouldReturnCorrectResult(
        IAction<Task> action1,
        IAction<int> action2)
    {
        // arrange
        var action = action1.SelectMany(() => action2);
        // act
        var actual = action.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task> -> IAction<Task<T>>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_Task_ReturningAsyncAction_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var currentResult = 0;
        var action = Actions.FromResult(Delay(() => { currentResult = value1; }))
            .SelectMany(() => Actions.FromResult(Task.FromResult(currentResult + value2)));
        // act
        var actual = await actor.When(action);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Task_Name_ReturningAsyncAction_ShouldReturnCorrectResult(
        IAction<Task> action1,
        IAction<Task<int>> action2)
    {
        // arrange
        var action = action1.SelectMany(() => action2);
        // act
        var actual = action.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task> -> IAction<Task>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_Task_ReturningAsyncTaskAction_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var result = 0;
        var action = Actions.FromResult(Delay(() => { result = value1; }))
            .SelectMany(() => Actions.FromResult(Delay(() => { result += value2; })));
        // act
        var task = actor.When(action);
        await task;
        var actual = result;
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Task_Name_ReturningAsyncTaskAction_ShouldReturnCorrectResult(
        IAction<Task> action1,
        IAction<Task> action2)
    {
        // arrange
        var action = action1.SelectMany(() => action2);
        // act
        var actual = action.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task> -> IQuestion<T>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_Task_ReturningQuestion_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var currentResult = 0;
        var question = Actions.FromResult(Delay(() => { currentResult = value1; }))
            .SelectMany(() => Questions.FromResult(currentResult + value2));
        // act
        var actual = await actor.AsksFor(question);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Task_Name_ReturningQuestion_ShouldReturnCorrectResult(
        IAction<Task> action1,
        IQuestion<int> question2)
    {
        // arrange
        var question = action1.SelectMany(() => question2);
        // act
        var actual = question.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    #region SelectMany: IAction<Task> -> IQuestion<Task<T>>

    [Theory, DomainAutoData]
    public async Task SelectMany_Async_Task_ReturningQuestionAsync_ShouldReturnCorrectResult(
        int value1,
        int value2)
    {
        // arrange
        var actor = new Actor("John");
        var currentResult = 0;
        var question = Actions.FromResult(Delay(() => { currentResult = value1; }))
            .SelectMany(() => Questions.FromResult(Delay(() => currentResult + value2)));
        // act
        var actual = await actor.AsksFor(question);
        // assert
        var expected = value1 + value2;
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void SelectMany_Async_Task_Name_ReturningQuestionAsync_ShouldReturnCorrectResult(
        IAction<Task> action1,
        IQuestion<Task<int>> question2)
    {
        // arrange
        var question = action1.SelectMany(() => question2);
        // act
        var actual = question.Name;
        // assert
        var expected = "[SelectMany] " + action1.Name;
        Assert.Equal(expected, actual);
    }

    #endregion

    private static async Task Delay(Action action)
    {
        await Task.Delay(1);
        action();
    }

    private static async Task<T> Delay<T>(Func<T> action)
    {
        await Task.Delay(1);
        return action();
    }

    #region Select

    [Theory, DomainAutoData]
    public void Select_ShouldReturnCorrectResult(IAction<string> action, Func<string, object> selector)
    {
        // act
        var actual = action.Select(selector);
        // assert
        var expected = new SelectAction<string, object>(action, selector);
        actual.Should().BeOfType<SelectAction<string, object>>();
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory, DomainAutoData]
    public async Task Select_Async_ShouldReturnCorrectResult(IAction<Task<string>> action, string value,
        object expected)
    {
        // arrange
        var selector = new Mock<Func<string, object>>();
        selector.Setup(s => s(value)).Returns(expected);
        // act
        var actual = action.Select(selector.Object);
        // assert            
        var selectAction = actual.Should().BeOfType<SelectActionAsync<string, object>>().Which;
        selectAction.Action.Should().Be(action);
        var actualSelected = await selectAction.Selector(value);
        Assert.Equal(expected, actualSelected);
    }

    [Theory, DomainAutoData]
    public void Select_Async_WithAsyncFunc_ShouldReturnCorrectResult(IAction<Task<string>> action,
        Func<string, Task<object>> selector)
    {
        // act
        var actual = action.Select(selector);
        // assert
        var expected = new SelectActionAsync<string, object>(action, selector);
        actual.Should().BeOfType<SelectActionAsync<string, object>>();
        actual.Should().BeEquivalentTo(expected);
    }

    #endregion

    #region Tagged

    [Theory, DomainAutoData]
    public void Tagged_ShouldReturnCorrectValue(IAction<object> action, string tag)
    {
        // arrange
        // act
        var actual = action.Tagged(tag);
        // assert
        var expected = Actions.CreateTagged(action.Name, (tag, action));
        actual.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
    }

    #endregion

    #region Named

    [Theory, DomainAutoData]
    public void Named_ExecuteShouldCallExecuteOnAction(
        IActor actor1,
        IActor actor2,
        IAction<object> action,
        string name,
        object expected1,
        object expected2)
    {
        // arrange
        var actorAction = Mock.Get(action);
        actorAction.Setup(a => a.ExecuteGivenAs(actor1)).Returns(expected1);
        actorAction.Setup(a => a.ExecuteWhenAs(actor2)).Returns(expected2);
        // act
        var actual = action.Named(name);
        var actualValue = (actual.ExecuteGivenAs(actor1), actual.ExecuteWhenAs(actor2));
        // assert
        Assert.Equal((expected1, expected2), actualValue);
    }

    [Theory, DomainAutoData]
    public void Named_NameShouldReturnCorrectValue(IAction<string> action, string expected)
    {
        // act
        var actual = action.Named(expected).Name;
        // assert
        Assert.Equal(expected, actual);
    }

    #endregion
}