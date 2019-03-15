using OpenQA.Selenium;
using System;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Identify a property that should be mapped to a UI element, and specify how the element is located
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TargetAttribute : Attribute
    {
        /// <summary>
        /// Initialize a new instance of <see cref="TargetAttribute"/>
        /// </summary>
        /// <param name="by">Define the method used to locate the element</param>
        /// <param name="value">The By value</param>
        public TargetAttribute(ByMethod by, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), ExceptionMessages.ArgumentCannotBeNullOrEmpty);
            }

            ByMethod = by;
            Value = value;
        }

        /// <summary>
        /// Gets the By method
        /// </summary>
        public ByMethod ByMethod { get; }
        /// <summary>
        /// Gets the By value
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// Gets the target name
        /// </summary>
        public string Name { get; set; }

        internal By GetSeleniumBy()
        {
            switch (ByMethod)
            {
                case ByMethod.CssSelector:
                    return By.CssSelector(Value);
                case ByMethod.Id:
                    return By.Id(Value);
                case ByMethod.Name:
                    return By.Name(Value);
                case ByMethod.ClassName:
                    return By.ClassName(Value);
                case ByMethod.TagName:
                    return By.TagName(Value);
                case ByMethod.XPath:
                    return By.XPath(Value);
                default:
                    throw new ArgumentOutOfRangeException($"The By method {ByMethod} is not valid");
            }
        }
    }
}
