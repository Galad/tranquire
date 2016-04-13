using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Base class for actions performed on a target
    /// </summary>
    public abstract class TargetedAction : Action<BrowseTheWeb>
    {
        /// <summary>
        /// Gets the target into which the action is executed
        /// </summary>
        public ITarget Target { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TargetedAction"/>
        /// </summary>
        /// <param name="target">The target into which the action is executed</param>
        protected TargetedAction(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            Target = target;
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="executor"></param>
        protected override void ExecuteWhen(IActor executor, BrowseTheWeb ability)
        {
            var element = Target.ResolveFor(ability.Driver);
            ExecuteAction(executor, element);
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="executor"></param>
        /// <param name="element"></param>
        protected abstract void ExecuteAction(IActor executor, IWebElement element);
    }
}
