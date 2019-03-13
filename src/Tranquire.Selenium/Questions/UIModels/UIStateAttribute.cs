using System;
using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// A base class that define how values are retrieved from the UI, using <see cref="UIState{T}"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class UIStateAttribute : Attribute
    {
        internal abstract IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture);

        internal static IQuestion<T> Apply<T, TUIState>(SingleUIState<string, TUIState> uiState, IConverters<T> converters, CultureInfo culture)
            where TUIState : SingleUIState<string, TUIState>
        {
            return uiState.WithCulture(culture).As<T>(converters);
        }

        internal static IQuestion<T> Apply<T, TUIState>(SingleUIState<bool, TUIState> uiState, IConverters<T> converters, CultureInfo culture)
            where TUIState : SingleUIState<bool, TUIState>
        {
            return uiState.WithCulture(culture).As<T>(converters);
        }
        internal static IQuestion<T> Apply<T, TUIState>(SingleUIState<ImmutableArray<string>, TUIState> uiState, IConverters<T> converters, CultureInfo culture)
            where TUIState : SingleUIState<ImmutableArray<string>, TUIState>
        {
            return uiState.WithCulture(culture).As<T>(converters);
        }
    }
}
