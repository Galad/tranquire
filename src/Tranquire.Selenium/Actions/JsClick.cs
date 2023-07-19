using OpenQA.Selenium;

namespace Tranquire.Selenium.Actions;

/// <summary>
/// Represent a click on a target with javascript
/// </summary>
public class JsClick : ActionBaseUnit<WebBrowser>, ITargeted
{
    /// <summary>
    /// Gets the target which will be clicked
    /// </summary>
    public ITarget Target { get; }
    /// <summary>
    /// Gets the action's name
    /// </summary>
    public override string Name => "Click on " + Target.Name + " in javascript";

    /// <summary>
    /// Creates a new instance of <see cref="JsClick"/>
    /// </summary>
    /// <param name="target"></param>
    public JsClick(ITarget target)
    {
        Target = target ?? throw new System.ArgumentNullException(nameof(target));
    }

    /// <summary>
    /// Creates the click action
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static JsClick On(ITarget target)
    {
        return new JsClick(target);
    }

    /// <summary>
    /// Click on the target
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="ability"></param>
    protected override void ExecuteWhen(IActor actor, WebBrowser ability)
    {
        var element = Target.ResolveFor(ability);
        ((IJavaScriptExecutor)ability.Driver).ExecuteScript("arguments[0].click();", element);
    }
}
