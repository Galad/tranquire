using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Questions.UIModels.Converters
{

    internal sealed class DoubleArrayConverters : IConverters<ImmutableArray<double>>
    {
        public ImmutableArray<double> Convert(string value, CultureInfo culture)
        {
            return ImmutableArray.Create(TextStateExtensions.DoubleConverter.Convert(value, culture));
        }

        public ImmutableArray<double> Convert(bool value, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert a boolean to a double array");
        }

        public ImmutableArray<double> Convert(ImmutableArray<string> value, CultureInfo culture)
        {
            return value.Select(v => TextStateExtensions.DoubleConverter.Convert(v, culture)).ToImmutableArray();
        }
    }
}
