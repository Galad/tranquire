using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Tranquire.Selenium.Actions.Selects
{
    public class SelectByMany<TValue> : Action
    {
        private readonly ISelectStrategy<TValue> _selectStrategy;
        private readonly ITarget _target;
        private readonly ImmutableArray<TValue> _values;

        public SelectByMany(
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

        protected override void ExecuteWhen(IActionCommand command, IActor actor)
        {
            if (_values.Length == 0)
            {
                return;
            }
            var element = _target.ResolveFor(actor);
            var selectElement = new SelectElement(element);
            foreach (var value in _values)
            {
                _selectStrategy.Select(selectElement, value);
            }
        }
    }
}