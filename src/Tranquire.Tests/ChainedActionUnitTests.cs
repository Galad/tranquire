using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Tranquire.Tests
{

    public class ChainedActionUnitTests : ActionTestsBase<ChainedActionUnitTests.ActionExecuteGiven, ChainedActionUnitTests.ActionExecuteWhen, ChainedActionUnitTests.ActionExecuteWhenAndGivenNotOverridden>
    {
        public class ActionExecuteWhen : ChainedAction<object>, IWithActor
        {
            public IActor Actor { get; private set; }
            private readonly IAction<object> _action;
            public override string CurrentActionName { get; }

            public ActionExecuteWhen(IAction<Unit> previousAction, IAction<object> action, string currentActionName) : base(previousAction)
            {
                _action = action;
                CurrentActionName = currentActionName;
            }

            protected override object ExecuteWhen(IActor actor)
            {
                Actor = actor;
                return actor.Execute(_action);
            }

            protected override object ExecuteGiven(IActor actor)
            {
                return new object();
            }
        }

        public class ActionExecuteGiven : ChainedAction<object>, IWithActor
        {
            public IActor Actor { get; private set; }
            private readonly IAction<object> _action;
            public override string CurrentActionName => "";

            public ActionExecuteGiven(IAction<Unit> previousAction, IAction<object> action) : base(previousAction)
            {
                if (action == null) throw new ArgumentNullException(nameof(action));
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor)
            {
                return new object();
            }

            protected override object ExecuteGiven(IActor actor)
            {
                Actor = actor;
                return actor.Execute(_action);
            }
        }

        public class ActionExecuteWhenAndGivenNotOverridden : ChainedAction<object>
        {
            private readonly IAction<object> _action;
            public override string CurrentActionName => "";

            public ActionExecuteWhenAndGivenNotOverridden(IAction<Unit> previousAction, IAction<object> action) : base(previousAction)
            {
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor)
            {
                return actor.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallPreviousActionResultBeforeAction(
          [Frozen] IAction<object> action,
          [Frozen] IAction<Unit> previousAction,
          ActionExecuteWhen sut,
          Mock<IActor> actor,
          object value)
        {
            //arrange
            var result = new List<int>();
            actor.Setup(a => a.Execute(previousAction)).Returns(() =>
            {
                result.Add(1);
                return Unit.Default;
            });
            actor.Setup(a => a.Execute(action)).Returns(() =>
            {
                result.Add(2);
                return value;
            });            
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            result.ShouldAllBeEquivalentTo(new[] { 1, 2 }, o => o.WithStrictOrdering());
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallPreviousActionResultBeforeAction(
            [Frozen] IAction<object> action,
            [Frozen] IAction<Unit> previousAction,
            ActionExecuteGiven sut,
            Mock<IActor> actor,
            object value)
        {
            //arrange
            var result = new List<int>();
            actor.Setup(a => a.Execute(previousAction)).Returns(() =>
            {
                result.Add(1);
                return Unit.Default;
            });
            actor.Setup(a => a.Execute(action)).Returns(() =>
            {
                result.Add(2);
                return value;
            });
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            result.ShouldAllBeEquivalentTo(new[] { 1, 2 }, o => o.WithStrictOrdering());
        }
        
        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            [Frozen] Mock<IAction<Unit>> action,
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

