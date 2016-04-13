using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent a <see cref="IAction"/> composed of several <see cref="IAction"/>
    /// </summary>
    public class Task<T> : IAction<T>
    {
        /// <summary>
        /// The list of actions to execute
        /// </summary>
        public IEnumerable<IAction<T>> Actions { get; }

        /// <summary>
        /// Create a new instance of <see cref="Task"/>
        /// </summary>
        /// <param name="actions">The list of actions to execute</param>
        public Task(IEnumerable<IAction<T>> actions)
        {
            Guard.ForNull(actions, nameof(actions));
            Actions = actions;
        }

        /// <summary>
        /// Create a new instance of <see cref="Task"/>
        /// </summary>
        /// <param name="actions">The list of actions to execute</param>
        public Task(params IAction<T>[] actions) : this(actions as IEnumerable<IAction<T>>)
        {
        }

        public void ExecuteGivenAs(IActor actor, T ability)
        {
            Execute(actor, (action) => action.ExecuteGivenAs(actor, ability));
        }
        
        public void ExecuteWhenAs(IActor actor, T ability)
        {
            Execute(actor, (action) => action.ExecuteWhenAs(actor, ability));
        }

        private void Execute(IActor actor, System.Action<IAction<T>> executeAction)
        {
            Guard.ForNull(actor, nameof(actor));
            foreach (var action in Actions)
            {
                executeAction(action);
            }
        }
    }
}
