using System.Collections.Generic;

namespace Tranquire.Selenium.Actions.Selects
{
    public class SelectBuilder
    {
        private readonly ITarget _target;
        public SelectBuilder(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            _target = target;
        }

        public SelectByValue<string> TheValue(string value)
        {
            return new SelectByValue<string>(_target, value, new SelectByValueStrategy());
        }

        public SelectByValues<string> TheValues(IEnumerable<string> values)
        {
            return new SelectByValues<string>(_target, values, new SelectByValueStrategy());
        }
    }
}