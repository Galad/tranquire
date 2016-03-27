using System;
using System.Collections.Immutable;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a relative target
    /// </summary>
    public class TargetByRelative : TargetByBase
    {
        public ITarget TargetSource { get; }

        public TargetByRelative(By by, ITarget targetSource):base(by)
        {
            Guard.ForNull(targetSource, nameof(targetSource));         
            TargetSource = targetSource;
        }

        protected override ISearchContext SearchContext(IActor actor)
        {
            return TargetSource.ResolveFor(actor);
        }
    }
}