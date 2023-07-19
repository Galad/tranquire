using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels.Converters;

internal sealed class StringArrayConverters : IConverters<ImmutableArray<string>>
{
    public ImmutableArray<string> Convert(string value, CultureInfo culture)
    {
        return ImmutableArray.Create(value);
    }

    public ImmutableArray<string> Convert(bool value, CultureInfo culture)
    {
        return ImmutableArray.Create(value.ToString(culture));
    }

    public ImmutableArray<string> Convert(ImmutableArray<string> value, CultureInfo culture)
    {
        return value;
    }
}
