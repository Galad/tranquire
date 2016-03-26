using OpenQA.Selenium;
using System;
using System.Collections.Immutable;
using System.Linq;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public class ManyUIState<T> : UIState<T>
    {
        private readonly Func<IWebElement, T> Resolve;
        public ManyUIState(ITarget target, IConverter<T> converter, Func<IWebElement, T> resolve): base (target, converter)
        {
            Resolve = resolve;
        }

        public IQuestion<ImmutableArray<string>> AsText()
        {
            return new Question<T, string>(Target, ResolveFor, Converter);
        }

        public IQuestion<ImmutableArray<int>> AsInteger()
        {
            return CreateQuestion<int>();
        }

        public IQuestion<ImmutableArray<DateTime>> AsDateTime()
        {
            return CreateQuestion<DateTime>();
        }

        public IQuestion<ImmutableArray<bool>> AsBoolean()
        {
            return CreateQuestion<bool>();
        }

        public IQuestion<ImmutableArray<T>> Value => CreateQuestion<T>();

        public IQuestion<ImmutableArray<TCustom>> As<TCustom>()
        {
            return CreateQuestion<TCustom>();
        }

        private IQuestion<ImmutableArray<TAnswer>> CreateQuestion<TAnswer>()
        {
            return new Question<T, TAnswer>(Target, ResolveFor, Converter);
        }

        protected override T ResolveFor(IWebElement element)
        {
            return Resolve(element);
        }

        private class Question<TSource, TConverted> : IQuestion<ImmutableArray<TConverted>>
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

            public ImmutableArray<TConverted> AnsweredBy(IActor actor)
            {
                var webElements = Target.ResoveAllFor(actor);
                return webElements.Select(w => Converter.Convert<TConverted>(WebElementResolver(w))).ToImmutableArray();
            }
        }
    }
}