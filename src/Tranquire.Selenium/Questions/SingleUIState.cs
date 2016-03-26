using OpenQA.Selenium;
using System;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public abstract class SingleUIState<T> : UIState<T>
    {
        public SingleUIState(ITarget target, IConverter<T> converter): base (target, converter)
        {
        }

        public IQuestion<string> AsText()
        {
            return new Question<T, string>(Target, ResolveFor, Converter);
        }

        public IQuestion<int> AsInteger()
        {
            return CreateQuestion<int>();
        }

        public IQuestion<DateTime> AsDate()
        {
            return CreateQuestion<DateTime>();
        }

        public IQuestion<bool> AsBoolean()
        {
            return CreateQuestion<bool>();
        }

        public IQuestion<T> Value => CreateQuestion<T>();

        public IQuestion<TCustom> As<TCustom>()
        {
            return CreateQuestion<TCustom>();
        }

        public ManyUIState<T> Many()
        {
            return new ManyUIState<T>(Target, Converter, ResolveFor);
        }

        private IQuestion<TAnswer> CreateQuestion<TAnswer>()
        {
            return new Question<T, TAnswer>(Target, ResolveFor, Converter);
        }

        private class Question<TSource, TConverted> : IQuestion<TConverted>
        {
            public Func<IWebElement, T> WebElementResolver
            {
                get;
            }

            public ITarget Target
            {
                get;
            }

            public IConverter<T> Converter
            {
                get;
            }

            public Question(ITarget target, Func<IWebElement, T> webElementResolver, IConverter<T> converter)
            {
                Guard.ForNull(target, nameof(target));
                Guard.ForNull(webElementResolver, nameof(webElementResolver));
                Guard.ForNull(converter, nameof(converter));
                Target = target;
                WebElementResolver = webElementResolver;
                Converter = converter;
            }

            public TConverted AnsweredBy(IActor actor)
            {
                var webElement = Target.ResolveFor(actor);
                var value = WebElementResolver(webElement);
                return Converter.Convert<TConverted>(value);
            }
        }
    }
}