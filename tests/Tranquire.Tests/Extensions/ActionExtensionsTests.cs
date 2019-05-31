using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{
    public class ActionExtensionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion) => assertion.Verify(typeof(ActionExtensions));

        private void TestIf<TAction, TFunc, TValue, TResult>(Func<TAction, TFunc, TValue, TResult> act)
        {
            //arrange
            var fixture = new Fixture().Customize(new DomainCustomization());
            var action = fixture.Freeze<TAction>();
            var predicate = fixture.Freeze<TFunc>();
            var defaultValue = fixture.Freeze<TValue>();
            var expected = fixture.Create<TResult>();
            //act
            var actual = act(action, predicate, defaultValue);
            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        #region If
        [Fact]
        public void If_ShouldReturnCorrectValue() => TestIf((IAction<object> a, Func<bool> p, object v) => ActionExtensions.If(a, p, v));

        [Fact]
        public void If_Unit_ShouldReturnCorrectValue() => TestIf((IAction<Unit> a, Func<bool> p, Unit v) => ActionExtensions.If(a, p));

        [Fact]
        public void If_PredicateAbility_ShouldReturnCorrectValue() => TestIf((IAction<object> a, Func<Ability, bool> p, object v) => ActionExtensions.If(a, p, v));

        [Fact]
        public void If_PredicateAbility_Unit_ShouldReturnCorrectValue() => TestIf((IAction<Unit> a, Func<Ability, bool> p, Unit v) => ActionExtensions.If(a, p));
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
        public void AsActionUnit_ExecuteGiven_ShouldCallExecuteGivenOnSourceAction(Mock<IAction<object>> action, IActor actor)
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
        public async Task Select_Async_ShouldReturnCorrectResult(IAction<Task<string>> action, string value, object expected)
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
        public void Select_Async_WithAsyncFunc_ShouldReturnCorrectResult(IAction<Task<string>> action, Func<string, Task<object>> selector)
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
}
