using FluentAssertions;
using Moq;
using AutoFixture.Idioms;
using System;
using Tranquire.ActionBuilders;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionBuilderExtensionsTests
    {
        /// <summary>
        /// Act as a relay to call the <see cref="ActionBuilderExtensions"/> methods, as AutoFixture is not able to generate a generic type with constraints properly
        /// </summary>
        public static class ActionBuilderExtensionsRelay
        {
            public static object Then(IActionBuilder source, IAction<Unit> nextAction) => ActionBuilderExtensions.Then(source, nextAction);
            public static object Then(
                IActionBuilderWithCurrentAction<IAction<object>, object> source,
                Func<ActionResult<IAction<object>, object>, IAction<Unit>> nextAction) => ActionBuilderExtensions.Then(source, nextAction);
            public static object Then(IActionBuilder source, IAction<int> nextAction) => ActionBuilderExtensions.Then(source, nextAction, 0);
            public static object Then(
                IActionBuilderWithCurrentAction<IAction<object>, object> source,
                Func<ActionResult<IAction<object>, object>, IAction<int>> nextAction) => ActionBuilderExtensions.Then(source, nextAction, 0);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ActionBuilderExtensionsRelay));
        }

        [Theory, DomainAutoData]
        public void Then_WithActionUnit_ShouldReturnCorrectValue(
            Mock<IActionBuilder> source,
            IAction<Unit> action,
            IActionBuilder<IAction<Unit>, Unit> expected
            )
        {
            //arrange
            source.Setup(s => s.Then<IAction<Unit>, Unit>(action)).Returns(expected);
            //act            
            var actual = ActionBuilderExtensions.Then(source.Object, action);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Then_OnActionBuilderWithCurrentAction_WithActionUnit_ShouldReturnCorrectValue(
            Mock<IActionBuilderWithCurrentAction<IAction<object>, object>> source,
            IAction<object> currentAction,
            IAction<Unit> action,
            IActionBuilderWithPreviousResult<IAction<Unit>, Unit, IAction<object>, object> expected
            )
        {
            //arrange
            source.Setup(s => s.Then<IAction<Unit>, Unit>(It.Is((Func<ActionResult<IAction<object>, object>, IAction<Unit>> f) => f(new ActionResult<IAction<object>, object>(currentAction, null)) == action)))
                  .Returns(expected);
            //act            
            var actual = ActionBuilderExtensions.Then(source.Object, _ => action);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Then_WithInferenceParameter_ShouldReturnCorrectValue(
            Mock<IActionBuilder> source,
            IAction<string> action,
            IActionBuilder<IAction<string>, string> expected
            )
        {
            //arrange
            source.Setup(s => s.Then<IAction<string>, string>(action)).Returns(expected);
            //act            
            var actual = ActionBuilderExtensions.Then(source.Object, action, default(string));
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Then_OnActionBuilderWithCurrentAction_WithInferenceParameter_ShouldReturnCorrectValue(
            Mock<IActionBuilderWithCurrentAction<IAction<object>, object>> source,
            IAction<object> currentAction,
            IAction<string> action,
            IActionBuilderWithPreviousResult<IAction<string>, string, IAction<object>, object> expected
            )
        {
            //arrange
            source.Setup(s => s.Then<IAction<string>, string>(It.Is((Func<ActionResult<IAction<object>, object>, IAction<string>> f) => f(new ActionResult<IAction<object>, object>(currentAction, null)) == action)))
                  .Returns(expected);
            //act            
            var actual = ActionBuilderExtensions.Then(source.Object, _ => action, default(string));
            //assert
            actual.Should().Be(expected);
        }
    }
}
