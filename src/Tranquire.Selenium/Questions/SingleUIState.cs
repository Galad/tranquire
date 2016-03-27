using OpenQA.Selenium;
using System;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Represent the UI state for a single element in the page
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public abstract class SingleUIState<T, TState> : UIState<T> where TState : SingleUIState<T, TState>
    {
        public SingleUIState(ITarget target): this (target, CultureInfo.CurrentCulture)
        {
        }

        public SingleUIState(ITarget target, CultureInfo culture) : base (target, culture)
        {
        }
                        
        public IQuestion<T> Value => CreateQuestion<T>(new GenericConverter<T, T>(t => t));

        /// <summary>
        /// Creates a question
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer</typeparam>
        /// <param name="converter">The converter used to convert the UI state value to the answer type</param>
        /// <returns>A <see cref="IQuestion{TAnswer}"/> instance</returns>
        public IQuestion<TAnswer> As<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return CreateQuestion(converter);
        }

        /// <summary>
        /// Specified that the state represent a list of values
        /// </summary>
        /// <returns></returns>
        public ManyUIState<T> Many()
        {
            return new ManyUIState<T>(Target, ResolveFor);
        }

        private IQuestion<TAnswer> CreateQuestion<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return new SingleQuestion<T, TAnswer>(Target, ResolveFor, converter, Culture);
        }

        /// <summary>
        /// Change the culture of the value
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public TState WithCulture(CultureInfo culture)
        {
            return CreateState(Target, culture);
        }

        protected abstract TState CreateState(ITarget target, CultureInfo culture);

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