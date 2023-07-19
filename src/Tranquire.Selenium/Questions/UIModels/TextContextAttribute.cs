using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels;

/// <summary>
/// Retrieve the value using <see cref="TextContent"/>
/// </summary>
public sealed class TextContentAttribute : UIStateAttribute
{
    internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
    {
        return Apply(TextContent.Of(target), converters, culture);
    }
}
