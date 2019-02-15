using OpenQA.Selenium;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a target of a <see cref="IWebElement"/>
    /// </summary>d
    [DebuggerDisplay("Target by web element : {Name}, {WebElement}")]
    public class TargetByWebElement : ITarget
    {
        /// <summary>
        /// Gets the web element that is returned by <see cref="ResolveFor(ISearchContext)"/> and <see cref="ResolveAllFor(ISearchContext)"/>
        /// </summary>
        public IWebElement WebElement { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TargetByWebElement"/>
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="name"></param>
        public TargetByWebElement(IWebElement webElement, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentNullException(nameof(name), ExceptionMessages.ArgumentCannotBeNullOrEmpty);
            }

            WebElement = webElement ?? throw new System.ArgumentNullException(nameof(webElement));
            Name = name;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ITarget RelativeTo(ITarget targetSource)
        {
            if (targetSource == null)
            {
                throw new System.ArgumentNullException(nameof(targetSource));
            }

            return this;
        }

        /// <inheritdoc />
        public IWebElement ResolveFor(ISearchContext searchContext)
        {
            if (searchContext == null)
            {
                throw new System.ArgumentNullException(nameof(searchContext));
            }

            return WebElement;
        }

        /// <inheritdoc />
        public ImmutableArray<IWebElement> ResolveAllFor(ISearchContext searchContext)
        {
            if (searchContext == null)
            {
                throw new System.ArgumentNullException(nameof(searchContext));
            }

            return ImmutableArray.Create(WebElement);
        }

        /// <inheritdoc />
        public override string ToString() => $"{Name} (web element: {WebElement.TagName})";
    }
}