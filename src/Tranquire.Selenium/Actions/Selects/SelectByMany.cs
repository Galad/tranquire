using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Tranquire.Selenium.Actions.Selects
{
    /// <summary>
    /// Represent an action used to select values on an element
    /// </summary>
    /// <typeparam name="TValue">The type of the value to select</typeparam>
    public class SelectByMany<TValue> : Action<BrowseTheWeb>
    {
        private readonly ISelectStrategy<TValue> _selectStrategy;
        private readonly ITarget _target;
        private readonly ImmutableArray<TValue> _values;

        /// <summary>
        /// Creates a new instance of <see cref="SelectByMany{TValue}"/>
        /// </summary>
        /// <param name="target">The element on which to perform the selection</param>
        /// <param name="values">The values to select</param>
        /// <param name="selectStrategy">The strategy used to select the element</param>
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

        /// <summary>
        /// Execute the selection
        /// </summary>
        /// <param name="actor"></param>
        protected override void ExecuteWhen(IActor actor, BrowseTheWeb ability)
        {
            if (_values.Length == 0)
            {
                return;
            }
            var element = _target.ResolveFor(ability);
            var selectElement = new SelectElement(element);
            foreach (var value in _values)
            {
                _selectStrategy.Select(selectElement, value);
            }
        }
    }
}