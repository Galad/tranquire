using Moq;

namespace Tranquire.Tests;


public static class MockActorExtensions
{
    public static void SetupExecuteWhen<T>(this Mock<IActor> actor)
    {
        actor.Setup(a => a.Execute(It.IsAny<IAction<T>>())).Returns((IAction<T> a) => a.ExecuteWhenAs(actor.Object));
        actor.SetupExecuteWhenUnit();
    }

    public static void SetupExecuteWhenUnit(this Mock<IActor> actor)
    {
        actor.Setup(a => a.Execute(It.IsAny<IAction<Unit>>())).Returns((IAction<Unit> a) => a.ExecuteWhenAs(actor.Object));
    }

    public static void SetupExecuteGiven<T>(this Mock<IActor> actor)
    {
        actor.Setup(a => a.Execute(It.IsAny<IAction<T>>())).Returns((IAction<T> a) => a.ExecuteGivenAs(actor.Object));
        actor.SetupExecuteGivenUnit();
    }

    public static void SetupExecuteGivenUnit(this Mock<IActor> actor)
    {
        actor.Setup(a => a.Execute(It.IsAny<IAction<Unit>>())).Returns((IAction<Unit> a) => a.ExecuteGivenAs(actor.Object));
    }
}
