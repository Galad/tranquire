using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Retrieve the value using <see cref="Presence"/>
    /// </summary>
    public sealed class PresenceAttribute : UIStateAttribute
    {
        internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
        {
            return Apply(Presence.Of(target), converters, culture);
        }
    }
}
