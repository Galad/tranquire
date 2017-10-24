using System;
using System.Globalization;

namespace Tranquire.Selenium.Questions.Converters
{
    /// <summary>
    /// A converter that uses the given conversion function
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TConverted"></typeparam>
    public class GenericConverter<TSource, TConverted> : IConverter<TSource, TConverted>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GenericConverter{TSource, TConverted}"/>
        /// </summary>
        /// <param name="convertFunction">The function used to perform the conversion</param>
        public GenericConverter(Func<TSource, TConverted> convertFunction) : this((v, c) => convertFunction(v))
        {
            Guard.ForNull(convertFunction, nameof(convertFunction));
        }

        /// <summary>
        /// Creates a new instance of <see cref="GenericConverter{TSource, TConverted}"/>
        /// </summary>
        /// <param name="convertFunction">The function used to perform the conversion with a <see cref="CultureInfo"/></param>
        public GenericConverter(Func<TSource, CultureInfo, TConverted> convertFunction)
        {
            Guard.ForNull(convertFunction, nameof(convertFunction));
            ConvertFunction = convertFunction;
        }

        /// <summary>
        /// Returns the conversion function
        /// </summary>
        public Func<TSource, CultureInfo, TConverted> ConvertFunction { get; }

        /// <summary>
        /// Perform the conversion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public TConverted Convert(TSource value, CultureInfo culture)
        {
            return ConvertFunction(value, culture);
        }
    }
}