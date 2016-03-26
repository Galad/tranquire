using OpenQA.Selenium;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public abstract class UIState<T>
    {
        public ITarget Target { get; }
        public IConverter<T> Converter { get; }

        public UIState(ITarget target, IConverter<T> converter)
        {
            Guard.ForNull(target, nameof(target));
            Guard.ForNull(converter, nameof(converter));
            Target = target;
            Converter = converter;
        }

        protected abstract T ResolveFor(IWebElement element);
    }
}