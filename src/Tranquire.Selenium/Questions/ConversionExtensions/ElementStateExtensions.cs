using System;
using System.Collections.Immutable;
using OpenQA.Selenium;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Provides extensions to convert a <see cref="IWebElement"/> to other types
    /// </summary>
    public static class ElementStateExtensions
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly IConverter<IWebElement, string> TextConverter = new GenericConverter<IWebElement, string>(w => w.Text);
        public static readonly IConverter<IWebElement, int> IntegerConverter = new GenericConverter<IWebElement, int>((w, c) => int.Parse(w.Text, c));
        public static readonly IConverter<IWebElement, bool> BooleanConverter = new GenericConverter<IWebElement, bool>(w => bool.Parse(w.Text));
        public static readonly IConverter<IWebElement, DateTime> DateTimeConverter = new GenericConverter<IWebElement, DateTime>((w, c) => DateTime.Parse(w.Text, c));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region Single

        /// <summary>
        /// Creates a question returning the <see cref="IWebElement"/> value as a string
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IQuestion<string> AsText<TState>(this SingleUIState<IWebElement, TState> state) where TState : SingleUIState<IWebElement, TState>
        {
            return state.As(TextConverter);
        }

        /// <summary>
        /// Creates a question returning the <see cref="IWebElement"/> value as an integer
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IQuestion<int> AsInteger<TState>(this SingleUIState<IWebElement, TState> state) where TState : SingleUIState<IWebElement, TState>
        {
            return state.As(IntegerConverter);
        }

        /// <summary>
        /// Creates a question returning the <see cref="IWebElement"/> value as a boolean
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IQuestion<bool> AsBoolean<TState>(this SingleUIState<IWebElement, TState> state) where TState : SingleUIState<IWebElement, TState>
        {
            return state.As(BooleanConverter);
        }

        /// <summary>
        /// Creates a question returning the <see cref="IWebElement"/> value as a DateTime
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IQuestion<DateTime> AsDateTime<TState>(this SingleUIState<IWebElement, TState> state) where TState : SingleUIState<IWebElement, TState>
        {
            return state.As(DateTimeConverter);
        }
        #endregion

        #region Many
        /// <summary>
        /// Creates a question returning the <see cref="IWebElement"/> values as strings
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<string>> AsText(this ManyUIState<IWebElement> state)
        {
            return state.As(TextConverter);
        }

        /// <summary>
        /// Creates a question returning the <see cref="IWebElement"/> values as integers
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<int>> AsInteger(this ManyUIState<IWebElement> state)
        {
            return state.As(IntegerConverter);
        }

        /// <summary>
        /// Creates a question returning the <see cref="IWebElement"/> values as booleans
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<bool>> AsBoolean(this ManyUIState<IWebElement> state)
        {
            return state.As(BooleanConverter);
        }

        /// <summary>
        /// Creates a question returning the <see cref="IWebElement"/> values as DateTimes
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<DateTime>> AsDateTime(this ManyUIState<IWebElement> state)
        {
            return state.As(DateTimeConverter);
        }
        #endregion
    }
}
