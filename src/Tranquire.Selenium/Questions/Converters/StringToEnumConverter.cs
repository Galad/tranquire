using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Questions.Converters
{
    public class StringToEnumConverter<T> : IConverter<string, T> where T : struct
    {
        private readonly HashSet<T> _enumValues;

        public StringToEnumConverter()
        {
            _enumValues = new HashSet<T>((IEnumerable<T>)Enum.GetValues(typeof(T)));
        }

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
