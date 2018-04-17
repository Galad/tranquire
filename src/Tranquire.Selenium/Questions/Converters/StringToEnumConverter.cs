using System;
using System.Collections.Generic;
using System.Globalization;

namespace Tranquire.Selenium.Questions.Converters
{
    /// <summary>
    /// Convert a string to an enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringToEnumConverter<T> : IConverter<string, T> where T : struct
    {
        private readonly HashSet<T> _enumValues;

        /// <summary>
        /// Creates a new instance of <see cref="StringToEnumConverter{T}"/>
        /// </summary>
        public StringToEnumConverter()
        {
            _enumValues = new HashSet<T>((IEnumerable<T>)Enum.GetValues(typeof(T)));
        }

        /// <summary>
        /// Convert a string to an enum
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="culture">The culture of the value</param>
        /// <returns>An enum value corresponding to the string</returns>
        public T Convert(string value, CultureInfo culture)
        {
            T result;
            if (Enum.TryParse(value, out result) && _enumValues.Contains(result))
            {
                return result;
            }
            throw new FormatException($"Cannot convert the value {value} to the enum of type {typeof(T)}");
        }
    }
}
