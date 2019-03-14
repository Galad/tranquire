using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Retrieve the value using <see cref="Visibility"/>
    /// </summary>
    public sealed class VisibilityAttribute : UIStateAttribute
    {
        internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
        {
            return Apply(Visibility.Of(target), converters, culture);
        }
    }
}
