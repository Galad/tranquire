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
    public class Task : IAction
    {
        /// <summary>
        /// Gets the actions executed by this task
        /// </summary>
        public IEnumerable<IAction> Actions { get; }

        /// <summary>
        /// Creates a new instance of task
        /// </summary>
        /// <param name="actions">The actions executed by this task</param>
        public Task(IEnumerable<IAction> actions)
        {
            Guard.ForNull(actions, nameof(actions));
            Actions = actions;
        }

        /// <summary>
        /// Creates a new instance of task
        /// </summary>
        /// <param name="actions">The actions executed by this task</param>
        public Task(params IAction[] actions) : this(actions as IEnumerable<IAction>)
        {
        }

        /// <summary>
        /// Execute all the actions
        /// </summary>
        /// <param name="actor"></param>
        public void ExecuteGivenAs(IActor actor)
        {
            ExecuteActions(actor);
        }
       
        /// <summary>
        /// Execute all the actions
        /// </summary>
        /// <param name="actor"></param>
        public void ExecuteWhenAs(IActor actor)
        {

            ExecuteActions(actor);
        }

        private void ExecuteActions(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            foreach (var task in Actions)
            {
                actor.Execute(task);
            }
        }
    }
}
