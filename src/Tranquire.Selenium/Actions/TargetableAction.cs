using System;

namespace Tranquire.Selenium.Actions;

/// <summary>
/// Base class for building actions performed on a target
/// </summary>
/// <typeparam name="TAction"></typeparam>
public abstract class TargetableAction<TAction> : ITargetableAction<TAction>
{
    /// <summary>
    /// Creates a new instance of <see cref="TargetableAction{TAction}"/>
    /// </summary>
    /// <param name="buildAction">A function used to build a new instance of the derived class</param>
    protected TargetableAction(Func<ITarget, TAction> buildAction)
    {
        BuildAction = buildAction ?? throw new ArgumentNullException(nameof(buildAction));
    }

    /// <summary>
    /// Gets the build action
    /// </summary>
    public Func<ITarget, TAction> BuildAction { get; }

    /// <summary>
    /// Creates a new action which will be performed on the given target
    /// </summary>
    /// <param name="target">The target to perform the action on</param>
    /// <returns>A new action</returns>
    public TAction Into(ITarget target)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        return BuildAction(target);
    }
}
