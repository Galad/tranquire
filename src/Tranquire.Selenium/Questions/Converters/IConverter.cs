using System.Globalization;

namespace Tranquire.Selenium.Questions.Converters
{
    public interface IConverter<TSource, TConverted>
    {
        TConverted Convert(TSource source, CultureInfo culture);
    }
}