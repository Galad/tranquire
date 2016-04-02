using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Tranquire.Selenium.Actions.Selects
{
    public class SelectByValues<TValue> : IAction
    {
        private readonly ISelectStrategy<TValue> _selectStrategy;
        private readonly ITarget _target;
        private readonly ImmutableArray<TValue> _values;

        public SelectByValues(
            ITarget target,
            IEnumerable<TValue> values,
            ISelectStrategy<TValue> selectStrategy)
        {
            Guard.ForNull(values, nameof(values));
            Guard.ForNull(target, nameof(target));
            Guard.ForNull(selectStrategy, nameof(selectStrategy));
            _values = values.ToImmutableArray();
            _target = target;
            _selectStrategy = selectStrategy;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            if (_values.Length == 0)
            {
                return actor;
            }
            var element = _target.ResolveFor(actor);
            var selectElement = new SelectElement(element);
            foreach (var value in _values)
            {
                _selectStrategy.Select(selectElement, value);
            }
            return actor;
        }
    }
}