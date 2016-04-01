using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions
{
    public class Select : IAction
    {
        private string _value;
        private ITarget _target;

        public Select(ITarget target, string value)
        {
            Guard.ForNull(target, nameof(value));
            _target = target;
            _value = value;
        }

        public static SelectBuilder On(ITarget target)
        {
            return new SelectBuilder(target);
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            var element = _target.ResolveFor(actor);
            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(_value);
            return actor;
        }
    }

    public class SelectBuilder
    {        
        private readonly ITarget _target;

        public SelectBuilder(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            _target = target;
        }

        public Select TheValue(string value)
        {
            return new Select(_target, value);
        }

        public SelectManyValues TheValues(IEnumerable<string> values)
        {
            return new SelectManyValues(_target, values);
        }
    }
}
