using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent a <see cref="IAction{TGiven, TWhen}"/> composed of several <see cref="IAction{TGiven, TWhen}"/>
    /// </summary>
    public class Task<T> : IAction
    {
        /// <summary>
        /// The list of actions to execute
        /// </summary>
        public IEnumerable<IAction<T, T>> Actions { get; }

        /// <summary>
        /// Create a new instance of <see cref="Task"/>
        /// </summary>
        /// <param name="actions">The list of actions to execute</param>
        public Task(IEnumerable<IAction<T, T>> actions)
        {
            Guard.ForNull(actions, nameof(actions));
            Actions = actions;
        }

        /// <summary>
        /// Create a new instance of <see cref="Task"/>
        /// </summary>
        /// <param name="actions">The list of actions to execute</param>
        public Task(params IAction<T, T>[] actions) : this(actions as IEnumerable<IAction<T, T>>)
        {
        }

        public void ExecuteGivenAs(IActor actor)
        {
            Execute(actor);
        }
        
        public void ExecuteWhenAs(IActor actor)
        {
            Execute(actor);
        }

        private void Execute(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            foreach (var action in Actions)
            {
                actor.Execute(action);
            }
        }
    }
}
