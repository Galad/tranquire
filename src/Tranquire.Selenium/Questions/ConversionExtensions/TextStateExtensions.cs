using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public static class TextStateExtensions
    {
        public static readonly IConverter<string, string> TextConverter = new GenericConverter<string, string>(s => s);
        public static readonly IConverter<string, int> IntegerConverter = new GenericConverter<string, int>((s, c) => int.Parse(s, c));
        public static readonly IConverter<string, bool> BooleanConverter = new GenericConverter<string, bool>(s => bool.Parse(s));
        public static readonly IConverter<string, DateTime> DateTimeConverter = new GenericConverter<string, DateTime>((s, c) => DateTime.Parse(s, c));

        #region Single

        public static IQuestion<string> AsText<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(TextConverter);
        }

        public static IQuestion<int> AsInteger<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(IntegerConverter);
        }

        public static IQuestion<bool> AsBoolean<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(BooleanConverter);
        }

        public static IQuestion<DateTime> AsDateTime<TState>(this SingleUIState<string, TState> state) where TState : SingleUIState<string, TState>
        {
            return state.As(DateTimeConverter);
        }

        public static IQuestion<T> AsEnum<TState, T>(this SingleUIState<string, TState> state) 
            where TState : SingleUIState<string, TState> 
            where T : struct
        {
            return state.As(new StringToEnumConverter<T>());
        }
        #endregion

        #region Many
        public static IQuestion<ImmutableArray<string>> AsText(this ManyUIState<string> state)
        {
            return state.As(TextConverter);
        }

        public static IQuestion<ImmutableArray<int>> AsInteger(this ManyUIState<string> state)
        {
            return state.As(IntegerConverter);
        }

        public static IQuestion<ImmutableArray<bool>> AsBoolean(this ManyUIState<string> state)
        {
            return state.As(BooleanConverter);
        }

        public static IQuestion<ImmutableArray<DateTime>> AsDateTime(this ManyUIState<string> state)
        {
            return state.As(DateTimeConverter);
        }

        public static IQuestion<ImmutableArray<T>> AsEnum<T>(this ManyUIState<string> state) where T : struct
        {
            return state.As(new StringToEnumConverter<T>());
        }
        #endregion
    }
}
