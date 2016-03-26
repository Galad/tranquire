using System;
using System.Collections.Immutable;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    internal class TargetByWebElement : ITarget
    {
        private readonly IWebElement _webElement;

        public TargetByWebElement(IWebElement webElement)
        {
            Guard.ForNull(webElement, nameof(webElement));
            _webElement = webElement;
        }

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