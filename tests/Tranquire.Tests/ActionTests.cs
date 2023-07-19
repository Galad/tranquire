using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Moq;
using Xunit;

namespace Tranquire.Tests;

public class ActionTests
{
    public class ActionExecuteWhen : ActionBase<object>
    {
        public IActor Actor;
        private readonly IAction<object> _action;
        public override string Name { get; }

        public ActionExecuteWhen(IAction<object> action, string name)
        {
            _action = action;
            Name = name;
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

    public class ActionExecuteGiven : ActionBase<object>
    {
        public IActor Actor;
        private readonly IAction<object> _action;
        public override string Name => "";

        public ActionExecuteGiven(IAction<object> action)
        {
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

    public class ActionExecuteWhenAndGivenNotOverridden : ActionBase<object>
    {
        private readonly IAction<object> _action;
        public override string Name => "";

        public ActionExecuteWhenAndGivenNotOverridden(IAction<object> action)
        {
            _action = action;
        }

        protected override object ExecuteWhen(IActor actor)
        {
            return actor.Execute(_action);
        }
    }

    [Theory, DomainAutoData]
    public void Sut_ShouldBeAction(ActionBase<object> sut)
    {
        Assert.IsAssignableFrom<IAction<object>>(sut);
    }

    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClauses(IFixture fixture)
    {
        var assertion = new GuardClauseAssertion(fixture);
        assertion.Verify(typeof(ActionExecuteGiven).GetMethods());
    }

    [Theory, DomainAutoData]
    public void ExecuteWhenAs_ShouldCallActorExecute(
        [Frozen] IAction<object> action,
        ActionExecuteWhen sut,
        Mock<IActor> actor,
        object expected)
    {
        //arrange
        actor.Setup(a => a.Execute(action)).Returns(expected);
        //act
        var actual = sut.ExecuteWhenAs(actor.Object);
        //assert
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void ExecuteGivenAs_ShouldCallActorExecute(
        [Frozen] IAction<object> action,
        ActionExecuteGiven sut,
        Mock<IActor> actor,
        object expected)
    {
        //arrange
        actor.Setup(a => a.Execute(action)).Returns(expected);
        //act
        var actual = sut.ExecuteGivenAs(actor.Object);
        //assert
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void ExecuteGivenAs_WhenExecuteGivenIsNotOverridden_ShouldCallActorExecute(
        [Frozen] IAction<object> action,
        ActionExecuteWhenAndGivenNotOverridden sut,
        Mock<IActor> actor,
        object expected)
    {
        //arrange
        actor.Setup(a => a.Execute(action)).Returns(expected);
        //act
        var actual = sut.ExecuteGivenAs(actor.Object);
        //assert
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void ExecuteWhenAs_ShouldUseCorrectActor(ActionExecuteWhen sut, Mock<IActor> actor)
    {
        //arrange
        var expected = actor.Object;
        //act
        sut.ExecuteWhenAs(actor.Object);
        //assert
        Assert.Equal(expected, sut.Actor);
    }

    [Theory, DomainAutoData]
    public void ExecuteGivenAs_ShouldUseCorrectActor(ActionExecuteGiven sut, Mock<IActor> actor)
    {
        //arrange
        var expected = actor.Object;
        //act
        sut.ExecuteGivenAs(actor.Object);
        //assert
        Assert.Equal(expected, sut.Actor);
    }

    [Theory, DomainAutoData]
    public void ToString_ShouldReturnCorrectValue(ActionExecuteWhen sut)
    {
        //arrange
        //act
        var actual = sut.ToString();
        //assert
        Assert.Equal(sut.Name, actual);
    }
}

