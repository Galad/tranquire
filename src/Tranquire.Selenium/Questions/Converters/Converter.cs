using System;
using System.Linq;

namespace Tranquire.Selenium.Questions.Converters
{
    public class Converter<T> : IConverter<T>
    {
        private readonly ICanConvert<T>[] _converters;
        protected Converter(params ICanConvert<T>[] converters)
        {
            _converters = converters;
        }

        public TConverted Convert<TConverted>(T value)
        {
            var converter = _converters.Select(c => c.CanConvert<TConverted>()).FirstOrDefault(c => c != null);
            if (converter == null)
            {
                throw new InvalidOperationException("No valid converter has been found");
            }

            return converter.Convert(value);
        }
    }
}