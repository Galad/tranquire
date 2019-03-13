using System;
using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels.Converters
{
    internal sealed class BooleanConverters : IConverters<bool>
    {
        public bool Convert(string value, CultureInfo culture)
        {
            return TextStateExtensions.BooleanConverter.Convert(value, culture);
        }

        public bool Convert(bool value, CultureInfo culture)
        {
            return value;
        }

        public bool Convert(ImmutableArray<string> value, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert a string array to a boolean");
        }
    }
}
