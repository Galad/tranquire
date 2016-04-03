using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent executable action
    /// </summary>
    public interface IPerformable
    {
        /// <summary>
        /// Execute the action with the given actor
        /// </summary>
        /// <typeparam name="T">The actor's type</typeparam>
        /// <param name="actor">The actor used to execute the action</param>
        /// <returns>The current <see cref="IActor"/> instance, allowing to chain calls</returns>
        T PerformAs<T>(T actor) where T : IActor;
    }
}
