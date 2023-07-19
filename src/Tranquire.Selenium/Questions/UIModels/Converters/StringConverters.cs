using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels.Converters;

internal sealed class StringConverters : IConverters<string>
{
    public string Convert(string value, CultureInfo culture)
    {
        return value;
    }

    public string Convert(bool value, CultureInfo culture)
    {
        return value.ToString(culture);
    }

    public string Convert(ImmutableArray<string> value, CultureInfo culture)
    {
        return string.Join(", ", value);
    }
}
