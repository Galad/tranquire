namespace Tranquire.Selenium.Questions.Converters
{
    public interface IConverter<TSource>
    {
        TConverted Convert<TConverted>(TSource value);
    }

    public interface IConverter<TSource, TConverted>
    {
        TConverted Convert(TSource source);
    }
}