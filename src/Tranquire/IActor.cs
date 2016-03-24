using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public interface IActor
    {
        IActor WasAbleTo(IPerformable performable);
        IActor AttemptsTo(IPerformable performable);
        IActor Can<T>(T doSomething) where T : IAbility<T>;
        TAbility AbilityTo<TAbility>() where TAbility : IAbility<TAbility>;
        TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question);
    }
}
