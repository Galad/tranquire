using System;
using System.Collections.Immutable;
using System.Linq;

namespace Tranquire;

/// <summary>
/// Contains utility functions for actions
/// </summary>
public static class Actions
{
    /// <summary>
    /// An empty action that does nothing
    /// </summary>
    public static readonly IAction<Unit> Empty = new EmptyAction();

    private sealed class EmptyAction : IAction<Unit>
    {
        public string Name => "Empty action";
        public Unit ExecuteGivenAs(IActor actor) => Unit.Default;
        public Unit ExecuteWhenAs(IActor actor) => Unit.Default;
        public override string ToString() => Name;
    }

    private sealed class ResultAction<T> : IAction<T>
    {
        private readonly T _result;

        public ResultAction(T result)
        {
            _result = result;
        }

        public string Name => "Returns " + _result.ToString();

        public T ExecuteGivenAs(IActor actor)
        {
            return _result;
        }

        public T ExecuteWhenAs(IActor actor)
        {
            return _result;
        }

        public override string ToString() => Name;
    }

    /// <summary>
    /// Creates an action that returns the given value
    /// </summary>
    /// <typeparam name="TResult">The value type</typeparam>
    /// <param name="result">The value returned by the action</param>
    /// <returns></returns>
    public static IAction<TResult> FromResult<TResult>(TResult result)
    {
        return new ResultAction<TResult>(result);
    }

    private sealed class DelegateAction : ActionBaseUnit
    {
        public override string Name { get; }
        public System.Action<IActor> Action { get; }

        public DelegateAction(string name, System.Action<IActor> action)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override void ExecuteWhen(IActor actor)
        {
            Action(actor);
        }
    }

    /// <summary>
    /// Creates an action that executes the given delegate
    /// </summary>
    /// <param name="name">The action name</param>
    /// <param name="action">The delegate to execute</param>
    /// <returns></returns>
    public static IAction<Unit> Create(string name, System.Action<IActor> action)
    {
        return new DelegateAction(name, action);
    }

    private sealed class DelegateAction<T> : ActionBaseUnit<T>
    {
        public override string Name { get; }
        public System.Action<IActor, T> Action { get; }

        public DelegateAction(string name, System.Action<IActor, T> action)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override void ExecuteWhen(IActor actor, T ability)
        {
            Action(actor, ability);
        }
    }

    /// <summary>
    /// Creates an action that executes the given delegate
    /// </summary>
    /// <param name="name">The action name</param>
    /// <param name="action">The delegate to execute</param>
    /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
    public static IAction<T, Unit> Create<T>(string name, System.Action<IActor, T> action)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        return new DelegateAction<T>(name, action);
    }

    private sealed class FuncAction<TResult> : ActionBase<TResult>
    {
        public override string Name { get; }
        public Func<IActor, TResult> Action { get; }

        public FuncAction(string name, Func<IActor, TResult> action)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override TResult ExecuteWhen(IActor actor)
        {
            return Action(actor);
        }
    }

    /// <summary>
    /// Creates an action that executes the given delegate and return its value
    /// </summary>
    /// <param name="name">The action name</param>
    /// <param name="action">The delegate to execute</param>
    /// <returns></returns>
    public static IAction<T> Create<T>(string name, Func<IActor, T> action)
    {
        return new FuncAction<T>(name, action);
    }

    private sealed class FuncAction<TAbility, TResult> : ActionBase<TAbility, TResult>
    {
        public override string Name { get; }
        public Func<IActor, TAbility, TResult> Action { get; }

        public FuncAction(string name, Func<IActor, TAbility, TResult> action)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override TResult ExecuteWhen(IActor actor, TAbility ability)
        {
            return Action(actor, ability);
        }
    }

    /// <summary>
    /// Creates an action that executes the given delegate and return its value
    /// </summary>
    /// <param name="name">The action name</param>
    /// <param name="action">The delegate to execute</param>
    /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
    public static IAction<TAbility, TResult> Create<TResult, TAbility>(string name, Func<IActor, TAbility, TResult> action)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        return new FuncAction<TAbility, TResult>(name, action);
    }

    /// <summary>
    /// Create an action that use the <paramref name="givenAction"/> when it is called with <see cref="Tranquire.ActionBase{TResult}.ExecuteGivenAs(IActor)"/>, and <paramref name="whenAction"/> when it is called with <see cref="Tranquire.ActionBase{TResult}.ExecuteWhenAs(IActor)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">The action name</param>
    /// <param name="givenAction">The action that is executed when the <see cref="Tranquire.ActionBase{TResult}.ExecuteGivenAs(IActor)"/> is called on the returned action</param>
    /// <param name="whenAction">The action that is executed when the <see cref="Tranquire.ActionBase{TResult}.ExecuteWhenAs(IActor)"/> is called on the returned action</param>
    /// <returns>Test</returns>
    public static DispatchAction<T> CreateDispatched<T>(
        string name,
        IAction<T> givenAction,
        IAction<T> whenAction)
    {
        return new DispatchAction<T>(name, givenAction, whenAction);
    }

    /// <summary>
    /// Creates an action that identifies the action to use based on a tag.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTag"></typeparam>
    /// <param name="actions"></param>        
    /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
    public static IAction<IActionTags<TTag>, T> CreateTagged<T, TTag>(params (TTag tag, IAction<T> action)[] actions)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        if (actions == null)
        {
            throw new ArgumentNullException(nameof(actions));
        }

        return new TaggedAction<T, TTag>(null, actions.ToImmutableDictionary(t => t.tag, t => t.action));
    }

    /// <summary>
    /// Creates an action that identifies the action to use based on a tag.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTag"></typeparam>
    /// <param name="name"></param>
    /// <param name="actions"></param>        
    /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
    public static IAction<IActionTags<TTag>, T> CreateTagged<T, TTag>(string name, params (TTag tag, IAction<T> action)[] actions)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (actions == null)
        {
            throw new ArgumentNullException(nameof(actions));
        }

        return new TaggedAction<T, TTag>(name, actions.ToImmutableDictionary(t => t.tag, t => t.action));
    }

    /// <summary>
    /// Represents an action that can be performed in different ways, depending on the context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTag"></typeparam>
    private sealed class TaggedAction<T, TTag> : ActionBase<IActionTags<TTag>, T>
    {
        public TaggedAction(string name, ImmutableDictionary<TTag, IAction<T>> actions)
        {
            Actions = actions;
            Name = name ?? $"Tagged action with {string.Join(", ", Actions.Keys.OrderBy(k => k))}";
        }

        public ImmutableDictionary<TTag, IAction<T>> Actions { get; }

        public override string Name { get; }

        protected override T ExecuteGiven(IActor actor, IActionTags<TTag> ability)
        {
            var bestTag = ability.FindBestGivenTag(Actions.Keys);
            var action = Actions[bestTag];
            return actor.Execute(action);
        }

        protected override T ExecuteWhen(IActor actor, IActionTags<TTag> ability)
        {
            var bestTag = ability.FindBestWhenTag(Actions.Keys);
            var action = Actions[bestTag];
            return actor.Execute(action);
        }
    }
}
