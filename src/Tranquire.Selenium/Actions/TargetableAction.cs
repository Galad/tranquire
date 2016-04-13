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
    public abstract class TargetableAction<TAction> : ITargetableAction<TAction> where TAction : IAction<BrowseTheWeb>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TargetableAction{TAction}"/>
        /// </summary>
        /// <param name="buildAction">A function used to build a new instance of the derived class</param>
        protected TargetableAction(Func<ITarget, TAction> buildAction)
        {
            Guard.ForNull(buildAction, nameof(buildAction));
            BuildAction = buildAction;
        }

        /// <summary>
        /// Gets the build action
        /// </summary>
        public Func<ITarget, TAction> BuildAction { get; }

        /// <summary>
        /// Creates a new action which will be performed on the given target
        /// </summary>
        /// <param name="target">The target to perform the action on</param>
        /// <returns>A new action</returns>
        public TAction Into(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            return BuildAction(target);
        }
    }
}
