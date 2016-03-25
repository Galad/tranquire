using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public class CompositePerformable : IAction, ITask
    {
        public IEnumerable<IPerformable> Actions { get; }

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
