using System;
using System.Diagnostics;
using System.Globalization;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    ///  Represent a target with parameters located by a <see cref="By"/> object
    /// </summary>
    [DebuggerDisplay("Parametarized target : {Name}. Value : {Value}")]
    public sealed class TargetByParameterizable : ITargetWithParameters
    {

        /// <summary>
        /// Creates a new instance of <see cref="TargetByParameterizable"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="createBy"></param>
        /// <param name="format"></param>
        public TargetByParameterizable(string name, Func<string, By> createBy, string format)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), ExceptionMessages.ArgumentCannotBeNullOrEmpty);
            }
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format), ExceptionMessages.ArgumentCannotBeNullOrEmpty);
            }

            CreateBy = createBy ?? throw new ArgumentNullException(nameof(createBy));
            Format = format;
            Name = name;
        }

        /// <summary>
        /// Gets the function used to create the locator
        /// </summary>
        public Func<string, By> CreateBy { get; }
        /// <summary>
        /// Gets the value used to create the locator
        /// </summary>
        public string Format { get; }
        /// <summary>
        /// Gets the target name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a target with the  given parameters
        /// </summary>
        /// <param name="parameters">The parameters to use to format the <see cref="Format"/></param>
        /// <returns></returns>
        public ITarget Of(params object[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return new TargetBy(CreateBy(string.Format(CultureInfo.InvariantCulture, Format, parameters)), Name);
        }
    }
}
