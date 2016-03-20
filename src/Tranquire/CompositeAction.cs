using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public class CompositeAction : IAction
    {
        private readonly IEnumerable<IAction> _actions;

        public CompositeAction(IEnumerable<IAction> actions)
        {
            _actions = actions;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            foreach(var action in _actions)
            {
                action.PerformAs(actor);
            }
            return actor;
        }
    }
}
