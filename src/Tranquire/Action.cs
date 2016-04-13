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
    public abstract class Action<T> : IAction<T>
    {
        public void ExecuteGivenAs(IActor actor, T ability)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteGiven(actor, ability);
        }

        public void ExecuteWhenAs(IActor actor, T ability)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteWhen(actor, ability);
        }

        protected abstract void ExecuteWhen(IActor actor, T ability);

        protected virtual void ExecuteGiven(IActor actor, T ability)
        {
            ExecuteWhen(actor, ability);
        }
    }

    /// <summary>
    /// Represent an action on the system
    /// </summary>
    public abstract class Action : IAction
    {
        public void ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteGiven(actor);
        }

        public void ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteWhen(actor);
        }
        
        protected abstract void ExecuteWhen(IActor actor);

        protected virtual void ExecuteGiven(IActor actor)
        {
            ExecuteWhen(actor);
        }
    }
}
