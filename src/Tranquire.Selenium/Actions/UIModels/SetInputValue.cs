using OpenQA.Selenium;

namespace Tranquire.Selenium.Actions.UIModels;

internal sealed class SetInputValue : ActionBaseUnit<WebBrowser>
{
    private const string Script =
        "arguments[0].value = arguments[1];" +
        "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));" +
        "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));";

    public SetInputValue(ITarget target, string value)
    {
        Target = target;
        Value = value;
    }

    public ITarget Target { get; }
    public string Value { get; }

    public override string Name => $"Set the value of {Target.Name} to '{Value}'";

    protected override void ExecuteWhen(IActor actor, WebBrowser ability)
    {
        var element = Target.ResolveFor(ability);
        ((IJavaScriptExecutor)ability.Driver).ExecuteScript(Script, element, Value);
    }
}
