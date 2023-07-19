using Tranquire.Selenium.Actions.Clicks;

namespace Tranquire.Selenium.Actions;

/// <summary>
/// Creates click actions
/// </summary>
public static class Click
{
    /// <summary>
    /// Returns a click action on the given target
    /// </summary>
    /// <param name="target">The target where to click</param>
    /// <returns></returns>
    public static ClickOnAction On(ITarget target)
    {
        return new ClickOnAction(target);
    }
}
