using OpenQA.Selenium;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Base class for the UI state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UIState<T>
    {
        /// <summary>
        /// Gets The target of the elemnt
        /// </summary>
        public ITarget Target { get; }
        /// <summary>
        /// Gets the culture of the value
        /// </summary>
        public CultureInfo Culture { get; }

        public UIState(ITarget target, CultureInfo culture)
        {
            Guard.ForNull(target, nameof(target));
            Guard.ForNull(culture, nameof(culture));
            Target = target;
            Culture = culture;
        }

        /// <summary>
        /// Resolve the value for the given <see cref="IWebElement"/>
        /// </summary>
        /// <param name="element">The element to resolve the value from</param>
        /// <returns></returns>
        protected abstract T ResolveFor(IWebElement element);        
    }
}