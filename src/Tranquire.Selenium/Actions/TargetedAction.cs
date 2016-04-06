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
    public abstract class TargetedAction : Action
    {
        public ITarget Target { get; }

        protected TargetedAction(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            Target = target;
        }

        protected override void ExecuteWhen(IActor actor)
        {
            var element = Target.ResolveFor(actor);
            ExecuteAction(actor, element);
        }

        protected abstract void ExecuteAction<T>(T actor, IWebElement element) where T : IActor;
    }
}
