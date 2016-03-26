using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Targets;

namespace Tranquire.Selenium
{
    public interface ITarget
    {
        IWebElement ResolveFor(IActor actor);
        ImmutableArray<IWebElement> ResoveAllFor(IActor actor);
    }
}
