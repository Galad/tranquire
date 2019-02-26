using OpenQA.Selenium;
using System;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Represent a question
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the web page</typeparam>
    /// <typeparam name="TConverted">The converterd type of the value</typeparam>
    /// <typeparam name="TAnswer">The final type of the answer</typeparam>
    internal abstract class WebBrowserQuestion<TSource, TConverted, TAnswer> : QuestionBase<WebBrowser, TAnswer>, ITargeted
    {
        public Func<IWebElement, TSource> WebElementResolver { get; }
        public ITarget Target { get; }
        public IConverter<TSource, TConverted> Converter { get; }
        public CultureInfo Culture { get; }

        protected WebBrowserQuestion(ITarget target, Func<IWebElement, TSource> webElementResolver, IConverter<TSource, TConverted> converter, CultureInfo culture)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            WebElementResolver = webElementResolver ?? throw new ArgumentNullException(nameof(webElementResolver));
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));
        }

        protected TConverted Convert(TSource value)
        {
            return Converter.Convert(value, Culture);
        }
    }
}