using System;

namespace Tranquire.Extensions;

/// <summary>
/// Allows to execute an action only if the result of the predicate is true
/// </summary>
/// <typeparam name="T">The return type of the action</typeparam>
public sealed class IfAction<T> : ActionBase<T>
{
    /// <summary>
    /// Gets the action to execute if the predicate is true
    /// </summary>
    public IAction<T> Action { get; }
    /// <summary>
    /// Gets the predicate
    /// </summary>
    public Func<bool> Predicate { get; }
    /// <summary>
    /// Gets the value which is returned by the action when the predicate is false
    /// </summary>
    public T DefaultValue { get; }

    /// <summary>
    /// Creates a new instance of <see cref="IfAction{T}"/>
    /// </summary>
    /// <param name="predicate">The predicate</param>
    /// <param name="action">The action to execute if the predicate is true</param>
    /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
    public IfAction(Func<bool> predicate, IAction<T> action, T defaultValue)
    {
        Action = action ?? throw new ArgumentNullException(nameof(action));
        Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        if (defaultValue == null)
        {
            throw new ArgumentNullException(nameof(defaultValue));
        }
        DefaultValue = defaultValue;
    }

    /// <summary>
    /// The action's name
    /// </summary>
    public override string Name => "[If] " + Action.Name;

    /// <summary>
    /// Execute when
    /// </summary>
    /// <param name="actor"></param>
    /// <returns></returns>
    protected override T ExecuteWhen(IActor actor)
    {
        if (Predicate())
        {
            return actor.Execute(Action);
        }
        return DefaultValue;
    }
}

/// <summary>
/// Allows to execute an action only if the result of the predicate is true
/// </summary>
/// <typeparam name="T">The return type of the action</typeparam>
/// <typeparam name="TAbility">The predicate ability type</typeparam>
public sealed class IfAction<TAbility, T> : ActionBase<TAbility, T>
{
    /// <summary>
    /// Gets the action to execute if the predicate is true
    /// </summary>
    public IAction<T> Action { get; }
    /// <summary>
    /// Gets the predicate
    /// </summary>
    public Func<TAbility, bool> Predicate { get; }
    /// <summary>
    /// Gets the value which is returned by the action when the predicate is false
    /// </summary>
    public T DefaultValue { get; }

    /// <summary>
    /// The action's name
    /// </summary>
    public override string Name => "[If] " + Action.Name;

    /// <summary>
    /// Creates a new instance of <see cref="IfAction{TAbility, T}"/>
    /// </summary>
    /// <param name="predicate">The predicate</param>
    /// <param name="action">The action to execute if the predicate is true</param>
    /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
    public IfAction(Func<TAbility, bool> predicate, IAction<T> action, T defaultValue)
    {
        Action = action ?? throw new ArgumentNullException(nameof(action));
        Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        if (defaultValue == null)
        {
            throw new ArgumentNullException(nameof(defaultValue));
        }
        DefaultValue = defaultValue;
    }

    /// <summary>
    /// Execute when
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="ability"></param>
    /// <returns></returns>
    protected override T ExecuteWhen(IActor actor, TAbility ability)
    {
        if (Predicate(ability))
        {
            return actor.Execute(Action);
        }
        return DefaultValue;
    }
}
