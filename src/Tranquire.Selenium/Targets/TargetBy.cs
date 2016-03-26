using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    public class TargetBy : ITarget
    {
        public By By { get; }

        public TargetBy(By by)
        {
            Guard.ForNull(by, nameof(by));
            By = by;
        }

        public IWebElement ResolveFor(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            //TODO: ensure that the web element is visible
            return actor.BrowseTheWeb().FindElement(By);            
        }

        public ImmutableArray<IWebElement> ResoveAllFor(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return actor.BrowseTheWeb().FindElements(By).ToImmutableArray();
        }
    }
}
