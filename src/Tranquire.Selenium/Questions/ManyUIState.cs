﻿using OpenQA.Selenium;
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Represent the UI state for a list of elements in the page
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ManyUIState<T> : UIState<T>, IQuestion<ImmutableArray<T>>
    {
        private readonly Func<IWebElement, T> Resolve;
        private readonly Lazy<IQuestion<ImmutableArray<T>>> _question;

        /// <summary>
        /// Creates a new instance of <see cref="ManyUIState{T}"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="resolve"></param>
        /// <param name="culture"></param>
        public ManyUIState(ITarget target, Func<IWebElement, T> resolve, CultureInfo culture)
            : base(target, culture)
        {
            Resolve = resolve;
            _question = new Lazy<IQuestion<ImmutableArray<T>>>(() => CreateQuestion(new GenericConverter<T, T>(t => t)));
        }
        
        /// <summary>
        /// Gets a question which returns the state
        /// </summary>
        [Obsolete("This property will be removed in the future. Cast this class as a IQuestion<ImmutableArray<T>> in order to get a question")]
        public IQuestion<ImmutableArray<T>> Value => _question.Value;
        
        /// <summary>
        /// Creates a question
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer elements</typeparam>
        /// <param name="converter">The converter used to convert the UI state value to the answer type</param>
        /// <returns></returns>
        public IQuestion<ImmutableArray<TAnswer>> As<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return CreateQuestion(converter);
        }

        private IQuestion<ImmutableArray<TAnswer>> CreateQuestion<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return new ManyQuestion<T, TAnswer>(Target, ResolveFor, converter, Culture);
        }

        /// <summary>
        /// Resolve the element state
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override T ResolveFor(IWebElement element)
        {
            return Resolve(element);
        }

        private class ManyQuestion<TSource, TConverted> : WebBrowserQuestion<TSource, TConverted, ImmutableArray<TConverted>>
        {
            public ManyQuestion(ITarget target,
                                Func<IWebElement, TSource> webElementResolver,
                                IConverter<TSource, TConverted> converter,
                                CultureInfo culture)
                : base(target, webElementResolver, converter, culture)
            {
            }

            protected override ImmutableArray<TConverted> Answer(IActor actor, WebBrowser ability)
            {
                var webElements = Target.ResolveAllFor(ability);
                return webElements.Select(w => Convert(WebElementResolver(w)))
                                  .ToImmutableArray();
            }

            /// <summary>
            /// Gets the question's name
            /// </summary>
            public override string Name => $"What are the state of the elements identified by {Target.ToString()} ?";
        }

        /// <summary>
        /// Returns the action's name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"What are the state of the elements identified by {Target.ToString()} ?";
        
        string INamed.Name => _question.Value.Name;

        ImmutableArray<T> IQuestion<ImmutableArray<T>>.AnsweredBy(IActor actor) => _question.Value.AnsweredBy(actor);
    }
}