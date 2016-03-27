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
    public abstract class TargetedAction : IAction
    {
        public ITarget Target { get; }

        protected TargetedAction(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            Target = target;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            var element = Target.ResolveFor(actor);
            ExecuteAction(actor, element);
            return actor;
        }

        protected abstract void ExecuteAction<T>(T actor, IWebElement element) where T : IActor;
    }
}
