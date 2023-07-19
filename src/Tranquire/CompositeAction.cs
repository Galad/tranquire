using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tranquire;

/// <summary>
/// Represent a <see cref="IAction{TResult}"/> composed of several <see cref="IAction{TResult}"/>
/// </summary>    
public abstract class CompositeAction : IAction<Unit>, IEnumerable<IAction<Unit>>
{
    private sealed class EmptyCompositeAction : CompositeAction
    {
        public override string Name => "Empty composite action";
    }

    private sealed class AnonymousCompositeAction : CompositeAction
    {
        public override string Name => "Multiple composite actions";
        public AnonymousCompositeAction(ImmutableArray<IAction<Unit>> actions) : base(actions)
        {
        }
    }

    /// <summary>
    /// Gets the actions executed by this composite action
    /// </summary>
    public ImmutableArray<IAction<Unit>> Actions { get; }
    /// <summary>
    /// Gets the action's name
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Creates a new instance of <see cref="CompositeAction"/>
    /// </summary>
    /// <param name="actions">The actions executed by this composite action</param>
    protected CompositeAction(ImmutableArray<IAction<Unit>> actions)
    {
        if (actions.IsDefault)
        {
            throw new ArgumentException("The argument must be initialized", nameof(actions));
        }
        Actions = actions;
    }

    /// <summary>
    /// Creates a new instance of <see cref="CompositeAction"/>
    /// </summary>
    /// <param name="actions">The actions executed by this composite action</param>
    protected CompositeAction(params IAction<Unit>[] actions) : this(ImmutableArray.Create(actions ?? throw new ArgumentNullException(nameof(actions))))
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="CompositeAction"/> using a builder
    /// </summary>
    /// <param name="compositeActionsBuilder">A function taking an empty task and returning a task containing the actions to execute</param>
    protected CompositeAction(Func<CompositeAction, CompositeAction> compositeActionsBuilder) : this(GetActionsFromCompositeActionBuilder(compositeActionsBuilder))
    {
    }

    private static ImmutableArray<IAction<Unit>> GetActionsFromCompositeActionBuilder(Func<CompositeAction, CompositeAction> compositeActionBuilder)
    {
        if (compositeActionBuilder == null)
        {
            throw new ArgumentNullException(nameof(compositeActionBuilder));
        }

        return compositeActionBuilder(new EmptyCompositeAction()).Actions;
    }

    /// <summary>
    /// Execute all the actions
    /// </summary>
    /// <param name="actor"></param>
    public Unit ExecuteGivenAs(IActor actor)
    {
        return ExecuteActions(actor);
    }

    /// <summary>
    /// Execute all the actions
    /// </summary>
    /// <param name="actor"></param>
    public Unit ExecuteWhenAs(IActor actor)
    {
        return ExecuteActions(actor);
    }

    private Unit ExecuteActions(IActor actor)
    {
        if (actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }

        foreach (var action in Actions)
        {
            actor.Execute(action);
        }
        return Unit.Default;
    }

    /// <summary>
    /// Returns a new <see cref="CompositeAction"/> with the given action appended to its action list
    /// </summary>
    /// <param name="action">The action to add</param>
    /// <returns>A new instance of <see cref="CompositeAction"/> containing <paramref name="action"/></returns>
    public CompositeAction And(IAction<Unit> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        return new AnonymousCompositeAction(Actions.Add(action));
    }

    /// <summary>
    /// Returns the action's name
    /// </summary>
    /// <returns>Returns the action's name</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Gets the enumerator for the <see cref="Actions"/>
    /// </summary>
    /// <returns></returns>
    public IEnumerator<IAction<Unit>> GetEnumerator() => (Actions as IEnumerable<IAction<Unit>>).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => (Actions as IEnumerable).GetEnumerator();
}
