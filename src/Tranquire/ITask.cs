using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an action performed by the actor. It is usually composed of other <see cref="ITask"/> or <see cref="IAction"/>
    /// </summary>
    public interface ITask : IPerformable
    {
    }
}
