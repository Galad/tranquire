using Tranquire.Selenium.Actions.Selects;

namespace Tranquire.Selenium.Actions;

/// <summary>
/// Configures a select action
/// </summary>
/// <typeparam name="TValue">The type of the value to select</typeparam>
public class SelectBuilder<TValue> : TargetableAction<SelectBy<TValue>>
{
    /// <summary>
    /// Creates a new instance of <see cref="SelectBuilder{TValue}"/>
    /// </summary>
    /// <param name="value">The value to select</param>
    /// <param name="strategy">The strategy used to perform the selection on the element</param>
    public SelectBuilder(TValue value, ISelectStrategy<TValue> strategy)
        : base(t => new SelectBy<TValue>(t, value, strategy))
    {
    }
}