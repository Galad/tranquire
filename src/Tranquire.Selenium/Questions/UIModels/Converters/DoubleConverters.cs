using System;
using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels.Converters
{
    internal sealed class DoubleConverters : IConverters<double>
    {
        public double Convert(string value, CultureInfo culture)
        {
            return TextStateExtensions.DoubleConverter.Convert(value, culture);
        }

        public double Convert(bool value, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert a boolean to an double");
        }

        public double Convert(ImmutableArray<string> value, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert a string array to an double");
        }
    }
}
