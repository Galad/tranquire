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
        /// The list of actions to execute
        /// </summary>
        public IEnumerable<IAction> Actions { get; }

        /// <summary>
        /// Create a new instance of <see cref="Task"/>
        /// </summary>
        /// <param name="actions">The list of actions to execute</param>
        public Task(IEnumerable<IAction> actions)
        {
            Guard.ForNull(actions, nameof(actions));
            Actions = actions;
        }

        /// <summary>
        /// Create a new instance of <see cref="Task"/>
        /// </summary>
        /// <param name="actions">The list of actions to execute</param>
        public Task(params IAction[] actions) : this(actions as IEnumerable<IAction>)
        {
        }

        public T ExecuteGivenAs<T>(T actor) where T : class, IActor
        {
            Guard.ForNull(actor, nameof(actor));
            foreach (var action in Actions)
            {
                action.ExecuteGivenAs(actor);
            }
            return actor;
        }

        public T ExecuteWhenAs<T>(T actor) where T : class, IActor
        {
            Guard.ForNull(actor, nameof(actor));
            foreach (var action in Actions)
            {
                action.ExecuteWhenAs(actor);
            }
            return actor;
        }
    }
}
