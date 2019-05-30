using System.Globalization;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Retrieve the value using <see cref="Classes"/>
    /// </summary>
    public sealed class ClassesAttribute : UIStateAttribute
    {
        /// <summary>
        /// Specifies that the value is retrieved by evaluating any of the classes contains the given class.
        /// In this case, the value returned is a boolean
        /// </summary>
        public string Contains { get; set; }

        internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
        {
            if (typeof(T) == typeof(bool) && Contains != null)
            {
                return Classes.Of(target).WithCulture(culture).Select(c => (T)(object)c.Contains(Contains));
            }
            return Apply(Classes.Of(target), converters, culture);
        }
    }
}
