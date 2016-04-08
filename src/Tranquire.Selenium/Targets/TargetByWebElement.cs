using System;
using System.Collections.Immutable;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a target of a <see cref="IWebElement"/>
    /// </summary>
    internal class TargetByWebElement : ITarget
    {
        private readonly IWebElement _webElement;

        public TargetByWebElement(IWebElement webElement, string name)
        {
            Guard.ForNull(webElement, nameof(webElement));
            Guard.ForNullOrEmpty(name, nameof(name));
            _webElement = webElement;
            Name = name;
        }

        public string Name { get; }

        public ITarget RelativeTo(ITarget targetSource)
        {
            return this;
        }

        public IWebElement ResolveFor(IActor actor)
        {
            return _webElement;
        }

        public ImmutableArray<IWebElement> ResoveAllFor(IActor actor)
        {
            return ImmutableArray.Create(_webElement);
        }
    }
}