namespace Tranquire.Selenium.Questions.Converters
{
    public interface ICanConvert<TSource>
    {
        IConverter<TSource, T> CanConvert<T>();
    }
}