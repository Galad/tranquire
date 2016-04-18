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
    public abstract class Action<T> : IAction<T, T>
    {
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        public void ExecuteGivenAs(IActor actor, T ability)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteGiven(actor, ability);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        public void ExecuteWhenAs(IActor actor, T ability)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteWhen(actor, ability);
        }

        /// <summary>
        /// Executes the action when the method when the execution is in a When context, or when no ExecuteGiven override has been provided
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected abstract void ExecuteWhen(IActor actor, T ability);

        /// <summary>
        /// Executes the action when the method when the execution is in a Given context.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
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
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public void ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteGiven(actor);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public void ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteWhen(actor);
        }

        /// <summary>
        /// Executes the action when the method when the execution is in a When context, or when no ExecuteGiven override has been provided
        /// </summary>
        /// <param name="actor"></param>
        protected abstract void ExecuteWhen(IActor actor);

        /// <summary>
        /// Executes the action when the method when the execution is in a Given context.
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void ExecuteGiven(IActor actor)
        {
            ExecuteWhen(actor);
        }
    }
}
