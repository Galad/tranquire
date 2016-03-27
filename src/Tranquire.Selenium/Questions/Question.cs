using OpenQA.Selenium;
using System;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    internal abstract class Question<TSource, TConverted, TAnswer> : IQuestion<TAnswer>
    {
        public Func<IWebElement, TSource> WebElementResolver { get; }
        public ITarget Target { get; }
        public IConverter<TSource, TConverted> Converter { get; }
        public CultureInfo Culture { get; }

        public Question(ITarget target, Func<IWebElement, TSource> webElementResolver, IConverter<TSource, TConverted> converter, CultureInfo culture)
        {
            Guard.ForNull(target, nameof(target));
            Guard.ForNull(webElementResolver, nameof(webElementResolver));
            Guard.ForNull(converter, nameof(converter));
            Guard.ForNull(culture, nameof(culture));
            Target = target;
            WebElementResolver = webElementResolver;
            Converter = converter;
            Culture = culture;
        }

        public abstract TAnswer AnsweredBy(IActor actor);

        protected TConverted Convert(TSource value)
        {
            return Converter.Convert(value, Culture);
        }
    }
}