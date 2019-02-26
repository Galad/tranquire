using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Actions.Selects
{
    /// <summary>
    /// Represent an action used to select a value on an element
    /// </summary>
    /// <typeparam name="TValue">The type of the value to select</typeparam>
    public sealed class SelectBy<TValue> : ActionBaseUnit<WebBrowser>
    {
        private readonly TValue _value;
        private readonly ITarget _target;
        private readonly ISelectStrategy<TValue> _selectStrategy;
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public override string Name => $"Select value on {_target.Name}";

        /// <summary>
        /// Creates a new instance of <see cref="SelectByMany{TValue}"/>
        /// </summary>
        /// <param name="target">The element on which to perform the selection</param>
        /// <param name="value">The value to select</param>
        /// <param name="selectStrategy">The strategy used to select the element</param>
        public SelectBy(
            ITarget target,
            TValue value,
            ISelectStrategy<TValue> selectStrategy)
        {
            _target = target ?? throw new System.ArgumentNullException(nameof(target));
            _value = value;
            _selectStrategy = selectStrategy ?? throw new System.ArgumentNullException(nameof(selectStrategy));
        }

        /// <summary>
        /// Execute the selection
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            var element = _target.ResolveFor(ability.Driver);
            var selectElement = new SelectElement(element);
            _selectStrategy.Select(selectElement, _value);
        }
    }
}
