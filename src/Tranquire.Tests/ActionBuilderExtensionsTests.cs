using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Idioms;
using Tranquire.ActionBuilders;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionBuilderExtensionsTests
    {
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
