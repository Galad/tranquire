using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Targets
{
    public interface IParameterizedTarget
    {
        IWebElement ResolveFor(IActor actor, params object[] parameters);
        ImmutableArray<IWebElement> ResoveAllFor(IActor actor, params object[] parameters);
    }
}
