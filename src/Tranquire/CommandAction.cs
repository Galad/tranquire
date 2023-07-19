using Tranquire.Reporting;

namespace Tranquire;

/// <summary>
/// Represent an action for a given context (Given or When)
/// </summary>
/// <typeparam name="T">The return type</typeparam>
public class CommandAction<T> : ActionBase<T>
{
    /// <summary>
    /// Creates a new instance of <see cref="CommandAction{T}"/>
    /// </summary>
    /// <param name="action">The action to execute</param>
    /// <param name="actionContext">The action context</param>
    public CommandAction(IAction<T> action, ActionContext actionContext)
    {
        Action = action;
        ActionContext = actionContext;
    }

    /// <inheritdoc />
    public override string Name => ActionContext.ToString() + " " + Action.Name;

    /// <summary>
    /// Gets the action to execute
    /// </summary>
    public IAction<T> Action { get; }

    /// <summary>
    /// Gets the action context
    /// </summary>
    public ActionContext ActionContext { get; private set; }

    /// <inheritdoc />
    protected override T ExecuteWhen(IActor actor)
    {
        return actor.Execute(Action);
    }
}
