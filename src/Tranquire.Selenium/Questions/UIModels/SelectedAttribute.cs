using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Retrieve the value using <see cref="Selected"/>
    /// </summary>
    public sealed class SelectedAttribute : UIStateAttribute
    {
        internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
        {
            return Apply(Selected.Of(target), converters, culture);
        }
    }
}
