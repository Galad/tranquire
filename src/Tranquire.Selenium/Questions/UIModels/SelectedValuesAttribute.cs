using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels;

/// <summary>
/// Retrieve the value using <see cref="SelectedValues"/>
/// </summary>
public sealed class SelectedValuesAttribute : UIStateAttribute
{
    internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
    {
        return Apply(SelectedValues.Of(target), converters, culture);
    }
}
