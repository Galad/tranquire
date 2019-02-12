using OpenQA.Selenium;
using System.Collections.Immutable;

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
        /// <param name="searchContext">The <see cref="IWebDriver"/> used to retrieved the element</param>
        /// <returns></returns>
        IWebElement ResolveFor(ISearchContext searchContext);
        /// <summary>
        /// Returns an array of <see cref="IWebElement"/> corresponding to all targets
        /// </summary>
        /// <param name="searchContext">The <see cref="IWebDriver"/> used to retrieved the element</param>
        /// <returns></returns>
        ImmutableArray<IWebElement> ResolveAllFor(ISearchContext searchContext);
        /// <summary>
        /// Specifies that this target should be resolved starting from the given target
        /// </summary>
        /// <param name="targetSource">The target containing this target</param>
        /// <returns></returns>
        ITarget RelativeTo(ITarget targetSource);
        /// <summary>
        /// Gets the target name
        /// </summary>
        string Name { get; }
    }
}
