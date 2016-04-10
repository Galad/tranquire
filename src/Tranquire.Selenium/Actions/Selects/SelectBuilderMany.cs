using System.Collections.Generic;
using Tranquire.Selenium.Actions.Selects;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Configures a select action of many values
    /// </summary>
    /// <typeparam name="TValue">The type of the value to select</typeparam>
    public class SelectBuilderMany<TValue> : TargetableAction<SelectByMany<TValue>>
    {
        /// <summary>
        /// Creates a new instance of <see cref="SelectBuilderMany{TValue}"/>
        /// </summary>
        /// <param name="value">The values to select</param>
        /// <param name="strategy">The strategy used to perform the selection on the element</param>
        public SelectBuilderMany(IEnumerable<TValue> value, ISelectStrategy<TValue> strategy)
            : base(t => new SelectByMany<TValue>(t, value, strategy))
        {
        }
    }
}