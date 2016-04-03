using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an ability
    /// </summary>
    public interface IAbility { }

    /// <summary>
    /// Represent an ability
    /// </summary>
    /// <typeparam name="T">The concrete type of the ability</typeparam>
    public interface IAbility<T> : IAbility where T : IAbility
    {
        //TODO: remove ? 
        /// <summary>
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        T AsActor(IActor actor);
    }
}
