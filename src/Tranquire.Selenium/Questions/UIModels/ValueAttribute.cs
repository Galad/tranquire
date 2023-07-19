using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels;

/// <summary>
/// Retrieve the value using <see cref="Value"/>
/// </summary>
public sealed class ValueAttribute : UIStateAttribute
{
    internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
    {
        return Apply(Value.Of(target), converters, culture);
    }
}
