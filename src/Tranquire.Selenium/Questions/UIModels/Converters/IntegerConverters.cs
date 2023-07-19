using System;
using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels.Converters;

internal sealed class IntegerConverters : IConverters<int>
{
    public int Convert(string value, CultureInfo culture)
    {
        return TextStateExtensions.IntegerConverter.Convert(value, culture);
    }

    public int Convert(bool value, CultureInfo culture)
    {
        throw new NotSupportedException("Cannot convert a boolean to an integer");
    }

    public int Convert(ImmutableArray<string> value, CultureInfo culture)
    {
        throw new NotSupportedException("Cannot convert a string array to an integer");
    }
}
