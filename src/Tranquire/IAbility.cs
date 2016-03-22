using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public interface IAbility { }

    public interface IAbility<T> : IAbility where T : IAbility
    {
        T AsActor(IActor actor);
    }
}
