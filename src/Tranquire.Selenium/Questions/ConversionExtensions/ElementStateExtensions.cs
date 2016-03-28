using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public static class ElementStateExtensions
    {
        public static readonly IConverter<IWebElement, string> TextConverter = new GenericConverter<IWebElement, string>(w => w.Text);
        public static readonly IConverter<IWebElement, int> IntegerConverter = new GenericConverter<IWebElement, int>((w, c) => int.Parse(w.Text, c));
        public static readonly IConverter<IWebElement, bool> BooleanConverter = new GenericConverter<IWebElement, bool>(w => bool.Parse(w.Text));
        public static readonly IConverter<IWebElement, DateTime> DateTimeConverter = new GenericConverter<IWebElement, DateTime>((w, c) => DateTime.Parse(w.Text, c));

        #region Single

        public static IQuestion<string> AsText<TState>(this SingleUIState<IWebElement, TState> state) where TState : SingleUIState<IWebElement, TState>
        {
            return state.As(TextConverter);
        }

        public static IQuestion<int> AsInteger<TState>(this SingleUIState<IWebElement, TState> state) where TState : SingleUIState<IWebElement, TState>
        {
            return state.As(IntegerConverter);
        }

        public static IQuestion<bool> AsBoolean<TState>(this SingleUIState<IWebElement, TState> state) where TState : SingleUIState<IWebElement, TState>
        {
            return state.As(BooleanConverter);
        }

        public static IQuestion<DateTime> AsDateTime<TState>(this SingleUIState<IWebElement, TState> state) where TState : SingleUIState<IWebElement, TState>
        {
            return state.As(DateTimeConverter);
        }
        #endregion

        #region Many
        public static IQuestion<ImmutableArray<string>> AsText(this ManyUIState<IWebElement> state)
        {
            return state.As(TextConverter);
        }

        public static IQuestion<ImmutableArray<int>> AsInteger(this ManyUIState<IWebElement> state)
        {
            return state.As(IntegerConverter);
        }

        public static IQuestion<ImmutableArray<bool>> AsBoolean(this ManyUIState<IWebElement> state)
        {
            return state.As(BooleanConverter);
        }

        public static IQuestion<ImmutableArray<DateTime>> AsDateTime(this ManyUIState<IWebElement> state)
        {
            return state.As(DateTimeConverter);
        }
        #endregion
    }
}
