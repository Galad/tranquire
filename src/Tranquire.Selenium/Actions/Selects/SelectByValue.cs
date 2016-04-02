using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions.Selects
{
    public sealed class SelectByValue<TValue> : IAction
    {
        private readonly TValue _value;
        private readonly ITarget _target;
        private readonly ISelectStrategy<TValue> _selectStrategy;

        public SelectByValue(
            ITarget target, 
            TValue value,
            ISelectStrategy<TValue> selectStrategy)
        {
            Guard.ForNull(target, nameof(value));            
            Guard.ForNull(selectStrategy, nameof(selectStrategy));
            _target = target;
            _value = value;
            _selectStrategy = selectStrategy;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            var element = _target.ResolveFor(actor);
            var selectElement = new SelectElement(element);
            _selectStrategy.Select(selectElement, _value);
            return actor;
        }
    }
}
