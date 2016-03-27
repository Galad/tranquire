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
    /// <summary>
    /// Represent a target in the web page
    /// </summary>
    public interface ITarget
    {
        /// <summary>
        /// Returns a <see cref="IWebElement"/> corresponding to the target
        /// </summary>
        /// <param name="actor">The actor used to retrieved the element</param>
        /// <returns></returns>
        IWebElement ResolveFor(IActor actor);
        /// <summary>
        /// Returns an array of <see cref="IWebElement"/> corresponding to all targets
        /// </summary>
        /// <param name="actor">The actor used to retrieved the element</param>
        /// <returns></returns>
        ImmutableArray<IWebElement> ResoveAllFor(IActor actor);
        /// <summary>
        /// Specifies that this target should be resolved starting from the given target
        /// </summary>
        /// <param name="targetSource">The target containing this target</param>
        /// <returns></returns>
        ITarget RelativeTo(ITarget targetSource);
    }
}
