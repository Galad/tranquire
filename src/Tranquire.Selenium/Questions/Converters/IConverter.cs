using System.Globalization;

namespace Tranquire.Selenium.Questions.Converters
{
    /// <summary>
    /// Convert a value
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TConverted"></typeparam>
    public interface IConverter<TSource, TConverted>
    {
        /// <summary>
        /// Conver the given value to the specified type.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="culture">The culture from which the value is formatted</param>
        /// <returns></returns>
        TConverted Convert(TSource value, CultureInfo culture);
    }
}