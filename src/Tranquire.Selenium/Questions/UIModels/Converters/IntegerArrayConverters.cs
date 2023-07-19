using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Tranquire.Selenium.Questions.UIModels.Converters;

internal sealed class IntegerArrayConverters : IConverters<ImmutableArray<int>>
{
    public ImmutableArray<int> Convert(string value, CultureInfo culture)
    {
        return ImmutableArray.Create(TextStateExtensions.IntegerConverter.Convert(value, culture));
    }

    public ImmutableArray<int> Convert(bool value, CultureInfo culture)
    {
        throw new NotSupportedException("Cannot convert a boolean to a integer array");
    }

    public ImmutableArray<int> Convert(ImmutableArray<string> value, CultureInfo culture)
    {
        return value.Select(v => TextStateExtensions.IntegerConverter.Convert(v, culture)).ToImmutableArray();
    }
}
