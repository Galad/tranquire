using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Base class for building actions performed on a target
    /// </summary>
    /// <typeparam name="TAction"></typeparam>
    public abstract class TargetableAction<TAction> : ITargetableAction<TAction> where TAction : IAction
    {
        public TargetableAction(Func<ITarget, TAction> buildAction)
        {
            Guard.ForNull(buildAction, nameof(buildAction));
            BuildAction = buildAction;
        }

        public Func<ITarget, TAction> BuildAction { get; }

        public TAction Into(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            return BuildAction(target);
        }
    }
}
