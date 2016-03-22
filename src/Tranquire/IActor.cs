using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public interface IActor
    {
        IActor WasAbleTo<TPerformable>(TPerformable performable) where TPerformable : IPerformable;
        IActor AttemptsTo<TPerformable>(TPerformable performable) where TPerformable : IPerformable;
        IActor Can<T>(T doSomething) where T : IAbility<T>;
        TAbility AbilityTo<TAbility>() where TAbility : IAbility<TAbility>;
        TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question);
    }
}
