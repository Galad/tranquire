using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tranquire.Selenium.Actions.Selects
{
    /// <summary>
    /// Represent an action used to select values on an element
    /// </summary>
    /// <typeparam name="TValue">The type of the value to select</typeparam>
    public class SelectByMany<TValue> : ActionUnit<WebBrowser>
    {
        private readonly ISelectStrategy<TValue> _selectStrategy;
        private readonly ITarget _target;
        private readonly ImmutableArray<TValue> _values;
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public override string Name => $"Select values of {_target.Name}";

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
            if (values == null)
            {
                throw new System.ArgumentNullException(nameof(values));
            }

            _values = values.ToImmutableArray();
            _target = target ?? throw new System.ArgumentNullException(nameof(target));
            _selectStrategy = selectStrategy ?? throw new System.ArgumentNullException(nameof(selectStrategy));
        }

        /// <summary>
        /// Execute the selection
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
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