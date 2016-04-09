using System;
using System.Collections.Immutable;
using OpenQA.Selenium;
using System.Diagnostics;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a relative target
    /// </summary>
    [DebuggerDisplay("Relative target : {TargetSource}; Name : {Name}, {By}")]
    public class TargetByRelative : TargetByBase
    {
        public ITarget TargetSource { get; }

        public TargetByRelative(By by, string name, ITarget targetSource) : base(by, name)
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