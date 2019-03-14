using System;
using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels.Converters
{
    internal sealed class DateTimeConverters : IConverters<DateTime>
    {
        public DateTime Convert(string value, CultureInfo culture)
        {
            return TextStateExtensions.DateTimeConverter.Convert(value, culture);
        }

        public DateTime Convert(bool value, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert a boolean to a DateTime");
        }

        public DateTime Convert(ImmutableArray<string> value, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert a string array to a DateTime");
        }
    }
}
