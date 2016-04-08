using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Globalization;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    ///  Represent a target with parameters located by a <see cref="By"/> object
    /// </summary>
    public sealed class TargetByParameterizable : ITargetWithParameters
    {
        public TargetByParameterizable(string name, Func<string, By> createBy, string value)
        {
            Guard.ForNull(createBy, nameof(createBy));
            Guard.ForNullOrEmpty(value, nameof(value));
            Guard.ForNullOrEmpty(name, nameof(name));
            CreateBy = createBy;
            Value = value;
            Name = name;
        }

        public Func<string, By> CreateBy { get; }
        public string Value { get; }
        public string Name { get; }

        public ITarget Of(params object[] parameters)
        {
            Guard.ForNull(parameters, nameof(parameters));
            return new TargetBy(CreateBy(string.Format(CultureInfo.InvariantCulture, Value, parameters)), Name);
        }
    }
}
