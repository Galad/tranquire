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
        /// <summary>
        /// Gets the source of the current target
        /// </summary>
        public ITarget TargetSource { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TargetByRelative"/>
        /// </summary>
        /// <param name="by"></param>
        /// <param name="name"></param>
        /// <param name="targetSource"></param>
        public TargetByRelative(By by, string name, ITarget targetSource) : base(by, name)
        {
            Guard.ForNull(targetSource, nameof(targetSource));
            TargetSource = targetSource;
        }

        /// <summary>
        /// Return the target source search context
        /// </summary>
        /// <param name="webDriver"></param>
        /// <returns></returns>
        protected override ISearchContext SearchContext(IWebDriver webDriver)
        {
            return TargetSource.ResolveFor(webDriver);
        }
    }
}