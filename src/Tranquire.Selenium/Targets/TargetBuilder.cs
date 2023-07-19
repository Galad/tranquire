using System;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets;

/// <summary>
/// Configures the targets
/// </summary>
public class TargetBuilder
{
    /// <summary>
    /// Gets the name of the target
    /// </summary>
    public string FriendlyName { get; }

    /// <summary>
    /// Creates a new instance of <see cref="TargetBuilder"/>
    /// </summary>
    /// <param name="friendlyName"></param>
    public TargetBuilder(string friendlyName)
    {
        FriendlyName = friendlyName ?? throw new ArgumentNullException(nameof(friendlyName));
    }

    /// <summary>
    /// Returns the target located with the given <see cref="By"/> object
    /// </summary>
    /// <param name="by">The location of the target</param>
    /// <returns></returns>
    public ITarget LocatedBy(By by)
    {
        if (by == null)
        {
            throw new ArgumentNullException(nameof(by));
        }

        return new TargetBy(by, FriendlyName);
    }

    /// <summary>
    /// Returns a target taking formatting parameters
    /// </summary>
    /// <param name="format">The format used to locate the target</param>
    /// <param name="createBy">A function taking the formatted located as input and returning the <see cref="By"/> object</param>
    /// <returns></returns>
    public ITargetWithParameters LocatedBy(string format, Func<string, By> createBy)
    {
        if (createBy == null)
        {
            throw new ArgumentNullException(nameof(createBy));
        }

        return new TargetByParameterizable(FriendlyName, createBy, format);
    }

    /// <summary>
    /// Returns a target taking parameters
    /// </summary>
    /// <param name="createBy">A function taking a value and returning the <see cref="By"/> object</param>
    /// <returns></returns>
    public ITargetWithParameters<T> LocatedBy<T>(Func<T, By> createBy)
    {
        if (createBy == null)
        {
            throw new ArgumentNullException(nameof(createBy));
        }

        return new TargetByParameterizable<T>(FriendlyName, createBy);
    }

    /// <summary>
    /// Returns <see cref="ITarget"/> from a <see cref="IWebElement"/>
    /// </summary>
    /// <param name="webElement"></param>
    /// <returns></returns>
    public ITarget LocatedByWebElement(IWebElement webElement)
    {
        if (webElement == null)
        {
            throw new ArgumentNullException(nameof(webElement));
        }

        return new TargetByWebElement(webElement, FriendlyName);
    }
}