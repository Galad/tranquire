using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tranquire.Selenium.Actions
{
    public class SelectManyValues:IAction
    {
        private readonly ITarget _target;
        private readonly IEnumerable<string> _values;

        public SelectManyValues(ITarget target, IEnumerable<string> values)
        {
            Guard.ForNull(values, nameof(values));
            Guard.ForNull(target, nameof(target));
            this._values = values;
            this._target = target;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            if (!_values.Any())
            {
                return actor;
            }            
            var element = _target.ResolveFor(actor);
            var selectElement = new SelectElement(element);
            foreach (var value in _values)
            {                
                selectElement.SelectByValue(value);
            }            
            return actor;
        }
    }
}