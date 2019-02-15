using OpenQA.Selenium;
using System.Globalization;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Base class for the UI state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UIState<T> : ITargeted
    {
        /// <summary>
        /// Gets The target of the elemnt
        /// </summary>
        public ITarget Target { get; }
        /// <summary>
        /// Gets the culture of the value
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Creates a new instance of <see cref="UIState{T}"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        protected UIState(ITarget target, CultureInfo culture)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            Culture = culture ?? throw new System.ArgumentNullException(nameof(culture));
        }

        /// <summary>
        /// Resolve the value for the given <see cref="IWebElement"/>
        /// </summary>
        /// <param name="element">The element to resolve the value from</param>
        /// <returns></returns>
        protected abstract T ResolveFor(IWebElement element);
    }
}