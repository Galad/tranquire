using System;
using System.Collections.Immutable;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Provides extensions for the UIState classes
    /// </summary>
    public static class TextStateExtensions
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly IConverter<string, string> TextConverter = new GenericConverter<string, string>(s => s);
        public static readonly IConverter<string, int> IntegerConverter = new GenericConverter<string, int>((s, c) => int.Parse(s, c));
        public static readonly IConverter<string, bool> BooleanConverter = new GenericConverter<string, bool>(s => bool.Parse(s));
        public static readonly IConverter<string, DateTime> DateTimeConverter = new GenericConverter<string, DateTime>((s, c) => DateTime.Parse(s, c));
        public static readonly IConverter<string, double> DoubleConverter = new GenericConverter<string, double>((s, c) => double.Parse(s, c));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region Single

        /// <summary>
        /// Creates a question returning the string value as a string
        /// </summary>
        /// <typeparam name="TState">The type of the state</typeparam>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<string> AsText<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(TextConverter);
        }

        /// <summary>
        /// Creates a question returning the string value as an integer
        /// </summary>
        /// <typeparam name="TState">The type of the state</typeparam>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<int> AsInteger<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(IntegerConverter);
        }

        /// <summary>
        /// Creates a question returning the string value as an integer
        /// </summary>
        /// <typeparam name="TState">The type of the state</typeparam>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<double> AsDouble<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(DoubleConverter);
        }

        /// <summary>
        /// Creates a question returning the string value as a boolean
        /// </summary>
        /// <typeparam name="TState">The type of the state</typeparam>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<bool> AsBoolean<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(BooleanConverter);
        }

        /// <summary>
        /// Creates a question returning the string value as a DateTime
        /// </summary>
        /// <typeparam name="TState">The type of the state</typeparam>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<DateTime> AsDateTime<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(DateTimeConverter);
        }

        /// <summary>
        /// Creates a question returning the string value as an enum
        /// </summary>
        /// <typeparam name="TState">The type of the state</typeparam>
        /// <typeparam name="T">The type of the enum</typeparam>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<T> AsEnum<TState, T>(this SingleUIState<string, TState> state)
            where TState : SingleUIState<string, TState>
            where T : struct
        {
            return state.As(new StringToEnumConverter<T>());
        }
        #endregion

        #region Many
        /// <summary>
        /// Creates a question returning the string values as strings
        /// </summary>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<string>> AsText(this ManyUIState<string> state)
        {
            return state.As(TextConverter);
        }

        /// <summary>
        /// Creates a question returning the string values as integers
        /// </summary>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<int>> AsInteger(this ManyUIState<string> state)
        {
            return state.As(IntegerConverter);
        }

        /// <summary>
        /// Creates a question returning the string values as integers
        /// </summary>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<double>> AsDouble(this ManyUIState<string> state)
        {
            return state.As(DoubleConverter);
        }

        /// <summary>
        /// Creates a question returning the string values as booleans
        /// </summary>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<bool>> AsBoolean(this ManyUIState<string> state)
        {
            return state.As(BooleanConverter);
        }

        /// <summary>
        /// Creates a question returning the string values as DatTimes
        /// </summary>
        /// <param name="state">The source state</param>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<DateTime>> AsDateTime(this ManyUIState<string> state)
        {
            return state.As(DateTimeConverter);
        }

        /// <summary>
        /// Creates a question returning the string values as enums
        /// </summary>
        /// <param name="state">The source state</param>
        /// <typeparam name="T">The type of the enum</typeparam>
        /// <returns></returns>
        public static IQuestion<ImmutableArray<T>> AsEnum<T>(this ManyUIState<string> state) where T : struct
        {
            return state.As(new StringToEnumConverter<T>());
        }
        #endregion
    }
}
