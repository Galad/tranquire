using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public interface IAction : IPerformable
    {
    }

    public interface ITask : IPerformable
    {
    }

    public interface IAbility { }

    public interface IAbility<T> : IAbility where T : IAbility
    {
        T AsActor(IActor actor);
    }

    public interface IQuestion<TAnswer>
    {
        TAnswer AnsweredBy(IActor actor);
    }

    public interface IActor
    {
        IActor WasAbleTo(IPerformable performable);
        IActor AttemptsTo(IPerformable performable);
        IActor Can<T>(T doSomething) where T : IAbility<T>;
        TAbility AbilityTo<TAbility>() where TAbility : IAbility<TAbility>;
        TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question);
    }

    public interface IPerformable
    {
        T PerformAs<T>(T actor) where T : IActor;
    }

    public interface IPerformsTask
    {
        void AttemptsTo<T>(T task) where T : IPerformable;
        TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question);
    }
}
