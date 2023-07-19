using OpenQA.Selenium;

namespace Tranquire.Selenium.Actions;

/// <summary>
/// Base class for actions performed on a target
/// </summary>
public abstract class TargetedAction : ActionBaseUnit<WebBrowser>, ITargeted
{
    /// <summary>
    /// Gets the target into which the action is executed
    /// </summary>
    public ITarget Target { get; }

    /// <summary>
    /// Creates a new instance of <see cref="TargetedAction"/>
    /// </summary>
    /// <param name="target">The target into which the action is executed</param>
    protected TargetedAction(ITarget target)
    {
        Target = target ?? throw new System.ArgumentNullException(nameof(target));
    }

    /// <summary>
    /// Execute the action
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="ability"></param>
    protected override void ExecuteWhen(IActor actor, WebBrowser ability)
    {
        var element = Target.ResolveFor(ability.Driver);
        ExecuteAction(actor, element);
    }

    /// <summary>
    /// Execute the action
    /// </summary>        
    /// <param name="actor"></param>
    /// <param name="element"></param>
    protected abstract void ExecuteAction(IActor actor, IWebElement element);
}
