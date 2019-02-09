﻿using OpenQA.Selenium;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// The base class for targets
    /// </summary>
    [DebuggerDisplay("Target {Name}, {By}")]
    public abstract class TargetByBase : ITarget
    {
        /// <summary>
        /// Gets the target locator
        /// </summary>
        public By By { get; }
        /// <summary>
        /// Gets the target name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TargetByBase"/>
        /// </summary>
        /// <param name="by"></param>
        /// <param name="name"></param>
        protected TargetByBase(By by, string name)
        {
            Guard.ForNull(by, nameof(by));
            Guard.ForNullOrEmpty(name, nameof(name));
            By = by;
            Name = name;
        }

        /// <summary>
        /// Specifies that this target should be resolved starting from the given target
        /// </summary>
        /// <param name="targetSource">The target containing this target</param>
        /// <returns></returns>
        public virtual ITarget RelativeTo(ITarget targetSource)
        {
            return new TargetByRelative(By, Name, targetSource);
        }

        /// <summary>
        /// Returns a <see cref="IWebElement"/> corresponding to the target
        /// </summary>
        /// <param name="webDriver">The <see cref="IWebDriver"/> used to retrieved the element</param>
        /// <returns></returns>
        public IWebElement ResolveFor(IWebDriver webDriver)
        {
            Guard.ForNull(webDriver, nameof(webDriver));
            return SearchContext(webDriver).FindElement(By);
        }

        /// <summary>
        /// Returns an array of <see cref="IWebElement"/> corresponding to all targets
        /// </summary>
        /// <param name="webDriver">The <see cref="IWebDriver"/> used to retrieved the element</param>
        /// <returns></returns>
        public ImmutableArray<IWebElement> ResoveAllFor(IWebDriver webDriver)
        {
            Guard.ForNull(webDriver, nameof(webDriver));
            return SearchContext(webDriver).FindElements(By).ToImmutableArray();
        }

        /// <summary>
        /// Gets the search context
        /// </summary>
        /// <param name="webDriver"></param>
        /// <returns></returns>
        protected abstract ISearchContext SearchContext(IWebDriver webDriver);

        /// <summary>
        /// Returns the action's name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name + $" ({By.ToString()})";
    }
}