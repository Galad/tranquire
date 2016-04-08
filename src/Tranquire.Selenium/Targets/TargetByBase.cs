using System;
using System.Collections.Immutable;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// The base class for targets
    /// </summary>
    public abstract class TargetByBase : ITarget
    {
        public By By { get; }
        public string Name { get; }

        public TargetByBase(By by, string name)
        {
            Guard.ForNull(by, nameof(by));
            Guard.ForNullOrEmpty(name, nameof(name));
            By = by;
            Name = name;
        }

        public ITarget RelativeTo(ITarget targetSource)
        {
            return new TargetByRelative(By, Name, targetSource);
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

        /// <summary>
        /// Gets the search context
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        protected abstract ISearchContext SearchContext(IActor actor);
    }
}