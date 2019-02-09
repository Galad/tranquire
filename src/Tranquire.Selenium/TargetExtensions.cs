using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Extension methods for <see cref="ITarget"/>
    /// </summary>
    public static class TargetExtensions
    {
        /// <summary>
        /// A ResolveFor overload that takes a search context instead of a web driver
        /// </summary>
        /// <param name="target">The target</param>
        /// <param name="searchContext">The search to use</param>
        /// <returns></returns>
        public static IWebElement ResolveFor(this ITarget target, ISearchContext searchContext)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (searchContext == null)
            {
                throw new ArgumentNullException(nameof(searchContext));
            }

            var webDriver = new SearchContextAsWebDriver(searchContext);
            return target.ResolveFor(webDriver);
        }

        /// <summary>
        /// A ResolveAllFor overload that takes a search context instead of a web driver
        /// </summary>
        /// <param name="target">The target</param>
        /// <param name="searchContext">The search to use</param>
        /// <returns></returns>
        public static ImmutableArray<IWebElement> ResolveAllFor(this ITarget target, ISearchContext searchContext)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (searchContext == null)
            {
                throw new ArgumentNullException(nameof(searchContext));
            }

            var webDriver = new SearchContextAsWebDriver(searchContext);
            return target.ResoveAllFor(webDriver);
        }

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
        private class SearchContextAsWebDriver : IWebDriver
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
        {
            private readonly ISearchContext searchContext;

            public SearchContextAsWebDriver(ISearchContext searchContext)
            {
                this.searchContext = searchContext;
            }

            public string Url { get; set; }

            public string Title => string.Empty;

            public string PageSource => string.Empty;

            public string CurrentWindowHandle => string.Empty;

            public ReadOnlyCollection<string> WindowHandles => new ReadOnlyCollection<string>(new string[] { });

            public void Close()
            {
                // Method intentionally left empty.
            }

            public void Dispose()
            {
                // Method intentionally left empty.
            }

            public IWebElement FindElement(By by)
            {
                return searchContext.FindElement(by);
            }

            public ReadOnlyCollection<IWebElement> FindElements(By by)
            {
                return searchContext.FindElements(by);
            }

            public IOptions Manage() => null;

            public INavigation Navigate() => null;

            public void Quit()
            {
                // Method intentionally left empty.
            }

            public ITargetLocator SwitchTo() => null;
        }
    }
}
