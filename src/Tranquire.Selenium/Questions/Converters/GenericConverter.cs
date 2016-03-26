using System;

namespace Tranquire.Selenium.Questions.Converters
{
    public class GenericConverter<TSource, TConverted> : ExplicitConverter<TSource, TConverted>
    {
        public GenericConverter(Func<TSource, TConverted> convertFunction)
        {
            Guard.ForNull(convertFunction, nameof(convertFunction));
            ConvertFunction = convertFunction;
        }

        public Func<TSource, TConverted> ConvertFunction
        {
            get;
            private set;
        }

        public override TConverted Convert(TSource source)
        {
            return ConvertFunction(source);
        }
    }
}