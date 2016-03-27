using OpenQA.Selenium;
using System;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public abstract class SingleUIState<T, TState> : UIState<T> where TState : SingleUIState<T, TState>
    {
        public SingleUIState(ITarget target): this (target, CultureInfo.CurrentCulture)
        {
        }

        public SingleUIState(ITarget target, CultureInfo culture) : base (target, culture)
        {
        }
                
        public IQuestion<T> Value => CreateQuestion<T>(new GenericConverter<T, T>(t => t));

        public IQuestion<TAnswer> As<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return CreateQuestion(converter);
        }

        public ManyUIState<T> Many()
        {
            return new ManyUIState<T>(Target, ResolveFor);
        }

        private IQuestion<TAnswer> CreateQuestion<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return new SingleQuestion<T, TAnswer>(Target, ResolveFor, converter, Culture);
        }

        public TState WithCulture(CultureInfo culture)
        {
            return CreateState(Target, culture);
        }

        public abstract TState CreateState(ITarget target, CultureInfo culture);

        private class SingleQuestion<TSource, TConverted> : Question<TSource, TConverted, TConverted>
        {
            public SingleQuestion(ITarget target, Func<IWebElement, TSource> webElementResolver, IConverter<TSource, TConverted> converter, CultureInfo culture)
                : base (target, webElementResolver, converter, culture)
            {
            }

            public override TConverted AnsweredBy(IActor actor)
            {
                var webElement = Target.ResolveFor(actor);
                var value = WebElementResolver(webElement);
                return Convert(value);
            }
        }
    }
}