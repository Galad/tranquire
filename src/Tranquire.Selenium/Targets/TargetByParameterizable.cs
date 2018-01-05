using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Globalization;

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
        /// <param name="value"></param>
        public TargetByParameterizable(string name, Func<string, By> createBy, string value)
        {
            Guard.ForNull(createBy, nameof(createBy));
            Guard.ForNullOrEmpty(value, nameof(value));
            Guard.ForNullOrEmpty(name, nameof(name));
            CreateBy = createBy;
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Gets the function used to create the locator
        /// </summary>
        public Func<string, By> CreateBy { get; }
        /// <summary>
        /// Gets the value used to create the locator
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// Gets the target name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a target with the  given parameters
        /// </summary>
        /// <param name="parameters">The parameters to use to format the <see cref="Value"/></param>
        /// <returns></returns>
        public ITarget Of(params object[] parameters)
        {
            Guard.ForNull(parameters, nameof(parameters));
            return new TargetBy(CreateBy(string.Format(CultureInfo.InvariantCulture, Value, parameters)), Name);
        }
    }
}
