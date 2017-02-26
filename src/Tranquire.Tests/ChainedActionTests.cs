using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace Tranquire.Tests
{
    public class ChainedActionTests : ActionTestsBase<ChainedActionTests.ActionExecuteGiven, ChainedActionTests.ActionExecuteWhen, ChainedActionTests.ActionExecuteWhenAndGivenNotOverridden>
    {
        public class ActionExecuteWhen : ChainedAction<string, object>, IWithActor
        {
            public IActor Actor { get; private set; }
            private readonly IAction<object> _action;            
            public override string CurrentActionName { get; }
            public string PreviousActionResult { get; private set; }

            public ActionExecuteWhen(IAction<string> previousAction, IAction<object> action, string currentActionName) : base(previousAction)
            {
                _action = action;
                CurrentActionName = currentActionName;
            }

            protected override object ExecuteWhen(IActor actor, string previousActionResult)
            {
                Actor = actor;
                PreviousActionResult = previousActionResult;
                return actor.Execute(_action);
            }

            protected override object ExecuteGiven(IActor actor, string previousResult)
            {
                return new object();
            }
        }

        public class ActionExecuteGiven : ChainedAction<string, object>, IWithActor
        {
            public IActor Actor { get; private set; }
            private readonly IAction<object> _action;
            public override string CurrentActionName => "";
            public object PreviousActionResult { get; private set; }

            public ActionExecuteGiven(IAction<string> previousAction, IAction<object> action) : base(previousAction)
            {
                if (action == null) throw new ArgumentNullException(nameof(action));
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor, string previousActionResult)
            {
                return new object();
            }

            protected override object ExecuteGiven(IActor actor, string previousActionResult)
            {
                Actor = actor;
                PreviousActionResult = previousActionResult;
                return actor.Execute(_action);
            }
        }

        public class ActionExecuteWhenAndGivenNotOverridden : ChainedAction<string, object>
        {
            private readonly IAction<object> _action;
            public override string CurrentActionName => "";
            public string PreviousActionResult { get; private set; }

            public ActionExecuteWhenAndGivenNotOverridden(IAction<string> previousAction, IAction<object> action) : base(previousAction)
            {
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor, string previousActionResult)
            {
                PreviousActionResult = previousActionResult;
                return actor.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldReturnPreviousActionResult(
            [Frozen] IAction<string> action,
            ActionExecuteWhen sut,
            Mock<IActor> actor,
            string expected)
        {
            //arrange
            actor.Setup(a => a.Execute(action)).Returns(expected);
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.PreviousActionResult);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldReturnPreviousActionResult(
            [Frozen] IAction<string> action,
            ActionExecuteGiven sut,
            Mock<IActor> actor,
            string expected)
        {
            //arrange
            actor.Setup(a => a.Execute(action)).Returns(expected);
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.PreviousActionResult);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_WhenIsNotOverridden_ShouldReturnPreviousActionResult(
            [Frozen] IAction<string> action,
            ActionExecuteWhenAndGivenNotOverridden sut,
            Mock<IActor> actor,
            string expected)
        {
            //arrange
            actor.Setup(a => a.Execute(action)).Returns(expected);
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.PreviousActionResult);
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            [Frozen] Mock<IAction<string>> action,
            ActionExecuteWhen sut,
            string previousActionName)
        {
            //arrange
            action.Setup(a => a.Name).Returns(previousActionName);
            //act
            var actual = sut.Name;
            //assert
            var expected = $"{previousActionName}, Then {sut.CurrentActionName}";
            actual.Should().Be(expected);
        }
    }
}
