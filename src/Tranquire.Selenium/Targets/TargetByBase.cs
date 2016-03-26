using System.Collections.Immutable;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    public abstract class TargetByBase : ITarget
    {
        public By By
        {
            get;
        }

        public TargetByBase(By by)
        {
            Guard.ForNull(by, nameof(by));
            By = by;
        }

        public ITarget RelativeTo(ITarget targetSource)
        {
            return new TargetByRelative(By, targetSource);
        }

        public IWebElement ResolveFor(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            //TODO: ensure that the web element is visible
            return SearchContext(actor).FindElement(By);
        }

        public ImmutableArray<IWebElement> ResoveAllFor(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return SearchContext(actor).FindElements(By).ToImmutableArray();
        }

        protected abstract ISearchContext SearchContext(IActor actor);
    }
}