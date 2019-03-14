using System;
using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Retrieve the value using <see cref="CssValue"/>
    /// </summary>
    public sealed class CssValueAttribute : UIStateAttribute
    {
        /// <summary>
        /// Initialize a new instance of <see cref="CssValueAttribute"/>
        /// </summary>
        /// <param name="propertyName">The CSS property name to retrieve the value from</param>
        public CssValueAttribute(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(ExceptionMessages.ArgumentCannotBeNullOrEmpty, nameof(propertyName));
            }

            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the CSS property name
        /// </summary>
        public string PropertyName { get; }

        internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
        {
            return Apply(CssValue.Of(target).AndTheProperty(PropertyName), converters, culture);
        }
    }
}
