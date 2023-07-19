using System;

namespace Tranquire.Selenium.Actions.Enters;

/// <summary>
/// Clears the target value and enter a new value 
/// </summary>
public class EnterNewValueBuilder : TargetableAction<CompositeAction>
{
    /// <summary>
    /// Creates a new instance of <see cref="EnterNewValueBuilder"/>
    /// </summary>
    /// <param name="value">The new value to enter</param>
    public EnterNewValueBuilder(string value) : base(t => new EnterNewValue(t, value))
    {
    }
}

/// <summary>
/// Clears the target value and enter a new value 
/// </summary>
#pragma warning disable CA1710
public class EnterNewValue : CompositeAction
#pragma warning restore CA1710
{
    /// <summary>
    /// Gets the target
    /// </summary>
    public ITarget Target { get; }
    /// <summary>
    /// Gets the value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new instance of <see cref="EnterNewValue"/>
    /// </summary>
    /// <param name="target">The target</param>
    /// <param name="value">The value to enter into the target</param>
    public EnterNewValue(ITarget target, string value)
        : base(t => t.And(Clear.TheValueOf(target)).And(Enter.TheValue(value).Into(target)))
    {
        Target = target ?? throw new ArgumentNullException(nameof(target));
        Value = value;
    }

    /// <summary>
    /// Gets the action's name
    /// </summary>
    public override string Name => $"Enter the new value {Value} into the target {Target.Name}";
}