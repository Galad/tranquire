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
    public sealed class TargetByParameterizable : ITargetWithParameters
    {
        public TargetByParameterizable(Func<string, By> createBy, string value)
        {
            Guard.ForNull(createBy, nameof(createBy));
            Guard.ForNullOrEmpty(value, nameof(value));
            CreateBy = createBy;
            Value = value;
        }

        public Func<string, By> CreateBy { get; }
        public string Value { get; }

        public ITarget Of(params object[] parameters)
        {
            Guard.ForNull(parameters, nameof(parameters));
            return new TargetBy(CreateBy(string.Format(CultureInfo.InvariantCulture, Value, parameters)));
        }
    }
}
