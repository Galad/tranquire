using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Retrieve the value using <see cref="HtmlAttribute"/>
    /// </summary>
    public sealed class HtmlAttributeAttribute : UIStateAttribute
    {
        /// <summary>
        /// Initialize a new instance of <see cref="HtmlAttributeAttribute"/>
        /// </summary>
        /// <param name="attributeName">The attribute name to retrive the value from</param>
        public HtmlAttributeAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// Gets the property name
        /// </summary>
        public string AttributeName { get; }

        internal override IQuestion<T> CreateQuestion<T>(ITarget target, IConverters<T> converters, CultureInfo culture)
        {
            return Apply(HtmlAttribute.Of(target).Named(AttributeName), converters, culture);
        }
    }
}
