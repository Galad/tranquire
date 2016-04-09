using System;
using System.Collections.Immutable;
using OpenQA.Selenium;
using System.Diagnostics;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a target of a <see cref="IWebElement"/>
    /// </summary>
    [DebuggerDisplay("Target by web element : {Name}, {WebElement}")]
    internal class TargetByWebElement : ITarget
    {
        public IWebElement WebElement { get; }

        public TargetByWebElement(IWebElement webElement, string name)
        {
            Guard.ForNull(webElement, nameof(webElement));
            Guard.ForNullOrEmpty(name, nameof(name));
            WebElement = webElement;
            Name = name;
        }

        public string Name { get; }

        public ITarget RelativeTo(ITarget targetSource)
        {
            return this;
        }

        public IWebElement ResolveFor(IActor actor)
        {
            return WebElement;
        }

        public ImmutableArray<IWebElement> ResoveAllFor(IActor actor)
        {
            return ImmutableArray.Create(WebElement);
        }
    }
}