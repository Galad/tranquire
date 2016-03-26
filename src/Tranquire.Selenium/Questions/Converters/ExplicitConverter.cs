namespace Tranquire.Selenium.Questions.Converters
{
    public abstract class ExplicitConverter<TSource, TConverted> : IConverter<TSource, TConverted>, ICanConvert<TSource>
    {
        public IConverter<TSource, T> CanConvert<T>()
        {
            if (typeof (T) == typeof (TConverted))
            {
                return (IConverter<TSource, T>)this;
            }

            return null;
        }

        public abstract TConverted Convert(TSource source);
    }
}