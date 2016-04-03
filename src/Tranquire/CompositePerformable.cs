using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent a <see cref="IPerformable"/> composed of several <see cref="IPerformable"/>
    /// </summary>
    public class CompositePerformable : IAction, ITask
    {
        /// <summary>
        /// The list of actions to execute
        /// </summary>
        public IEnumerable<IPerformable> Actions { get; }

        /// <summary>
        /// Create a new instance of <see cref="CompositePerformable"/>
        /// </summary>
        /// <param name="actions">The list of actions to execute</param>
        public CompositePerformable(IEnumerable<IPerformable> actions)
        {
            Guard.ForNull(actions, nameof(actions));
            Actions = actions;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            Guard.ForNull(actor, nameof(actor));
            foreach(var action in Actions)
            {
                action.PerformAs(actor);
            }            
            return actor;
        }
    }
}
