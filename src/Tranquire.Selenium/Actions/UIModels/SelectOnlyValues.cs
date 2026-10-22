using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Actions.UIModels;

internal sealed class SelectOnlyValues : ActionBaseUnit<WebBrowser>
{
    public SelectOnlyValues(ITarget target, IReadOnlyList<string> values)
    {
        Target = target;
        Values = values;
    }

    public ITarget Target { get; }
    public IReadOnlyList<string> Values { get; }

    public override string Name => $"Select only the values [{string.Join(", ", Values)}] of {Target.Name}";

    protected override void ExecuteWhen(IActor actor, WebBrowser ability)
    {
        var select = new SelectElement(Target.ResolveFor(ability));
        if (select.IsMultiple)
            select.DeselectAll();
        foreach (var value in Values)
            select.SelectByValue(value);
    }
}
