using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an action on the system
    /// </summary>
    public abstract class Action : IAction
    {
        public IActor ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteGiven(actor);
            return actor;
        }

        public IActor ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteWhen(actor);
            return actor;
        }

        protected abstract void ExecuteWhen(IActor actor);

        protected virtual void ExecuteGiven(IActor actor)
        {
            ExecuteWhen(actor);
        }
    }
}
