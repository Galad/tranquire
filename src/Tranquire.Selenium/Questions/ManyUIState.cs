using OpenQA.Selenium;
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public class ManyUIState<T> : UIState<T>
    {
        private readonly Func<IWebElement, T> Resolve;
        public ManyUIState(ITarget target, Func<IWebElement, T> resolve)
            : this(target, resolve, CultureInfo.CurrentCulture)
        {
        }

        public ManyUIState(ITarget target, Func<IWebElement, T> resolve, CultureInfo culture)
            : base(target, culture)
        {
            Resolve = resolve;
        }

        public IQuestion<ImmutableArray<T>> Value => CreateQuestion<T>(new GenericConverter<T, T>(t => t));

        public IQuestion<ImmutableArray<TAnswer>> As<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return CreateQuestion(converter);
        }

        private IQuestion<ImmutableArray<TAnswer>> CreateQuestion<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return new ManyQuestion<T, TAnswer>(Target, ResolveFor, converter, Culture);
        }

        protected override T ResolveFor(IWebElement element)
        {
            return Resolve(element);
        }

        private class ManyQuestion<TSource, TConverted> : Question<TSource, TConverted, ImmutableArray<TConverted>>
        {
            public ManyQuestion(ITarget target,
                                Func<IWebElement, TSource> webElementResolver,
                                IConverter<TSource, TConverted> converter,
                                CultureInfo culture)
                : base(target, webElementResolver, converter, culture)
            {
            }

            public override ImmutableArray<TConverted> AnsweredBy(IActor actor)
            {
                var webElements = Target.ResoveAllFor(actor);
                return webElements.Select(w => Convert(WebElementResolver(w)))
                                  .ToImmutableArray();
            }
        }
    }
}