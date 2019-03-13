using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Retrieve the value using <see cref="Classes"/>
    /// </summary>
    public sealed class ClassesAttribute : UIStateAttribute
    {
        internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
        {
            return Apply(Classes.Of(target), converters, culture);
        }
    }
}
