using OpenQA.Selenium;

namespace Tranquire.Selenium.Actions.Enters;

/// <summary>
/// Enter a value in a target
/// </summary>
public class EnterValue : TargetedAction
{
    /// <summary>
    /// Gets the value to enter
    /// </summary>
    public string Value { get; }
    /// <summary>
    /// Gets the action's name
    /// </summary>
    public override string Name => $"Enter the value '{Value}' into '{Target.Name}'";

    /// <summary>
    /// Creates a new instance of <see cref="EnterValue"/>
    /// </summary>
    /// <param name="value">The value to enter</param>
    /// <param name="target">The target on which the value is entered</param>
    public EnterValue(string value, ITarget target)
        : base(target)
    {
        Value = value ?? throw new System.ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Enter the value
    /// </summary>        
    /// <param name="actor"></param>
    /// <param name="element"></param>
    protected override void ExecuteAction(IActor actor, IWebElement element)
    {
        element.SendKeys(Value);
    }

    /// <summary>
    /// Returns the action's name
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Enter the value '{Value}' into '{Target.Name}'";
}