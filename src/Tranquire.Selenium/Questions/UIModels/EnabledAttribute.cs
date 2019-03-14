using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Retrieve the value using <see cref="Enabled"/>
    /// </summary>
    public sealed class EnabledAttribute : UIStateAttribute
    {
        internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
        {
            return Apply(Enabled.Of(target), converters, culture);
        }
    }
}
