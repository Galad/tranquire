using OpenQA.Selenium;
using Tranquire.Selenium.Targets;

namespace Tranquire.Selenium;

/// <summary>
/// Creates targets
/// </summary>
public static class Target
{
    /// <summary>
    /// Creates a new target.
    /// </summary>
    /// <param name="friendlyName">A friendly name for the target</param>
    /// <returns>A <see cref="TargetBuilder"/> used to configure the target</returns>
    public static TargetBuilder The(string friendlyName)
    {
        return new TargetBuilder(friendlyName);
    }

    /// <summary>
    /// A target that locates the page
    /// </summary>
    /// <returns></returns>
    public static ITarget ThePage { get; } = Target.The("Page").LocatedBy(By.TagName("html"));

    /// <summary>
    /// A target that locates the page
    /// </summary>
    /// <returns></returns>
    public static ITarget TheBody { get; } = Target.The("Page").LocatedBy(By.TagName("body"));
}
