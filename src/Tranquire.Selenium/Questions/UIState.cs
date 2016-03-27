using OpenQA.Selenium;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public abstract class UIState<T>
    {
        public ITarget Target { get; }
        public CultureInfo Culture { get; }

        public UIState(ITarget target, CultureInfo culture)
        {
            Guard.ForNull(target, nameof(target));
            Guard.ForNull(culture, nameof(culture));
            Target = target;
            Culture = culture;
        }

        protected abstract T ResolveFor(IWebElement element);        
    }
}