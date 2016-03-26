using System;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{    
    public class TargetBuilder : ITargetBuilder
    {
        public string FriendlyName { get; }

        public TargetBuilder(string friendlyName)
        {
            this.FriendlyName = friendlyName ?? string.Empty;
        }

        public ITarget LocatedBy(By by)
        {
            Guard.ForNull(by, nameof(by));
            return new TargetBy(by);
        }

        public ITargetWithParameters LocatedBy(string formatValue, Func<string, By> createBy)
        {
            Guard.ForNull(createBy, nameof(createBy));
            Guard.ForNull(formatValue, nameof(formatValue));
            return new TargetByParameterizable(createBy, formatValue);
        }
    }
}