using System;
using System.Collections.Immutable;

namespace Tranquire.ActionBuilders
{
    /// <summary>
    /// An action builder with a current action
    /// </summary>
    /// <typeparam name="TAction">The action type</typeparam>
    /// <typeparam name="TResult">The action result type</typeparam>    
    public sealed class ActionBuilder<TAction, TResult> : IActionBuilder<TAction, TResult>
        where TAction : class, IAction<TResult>
    {
        private readonly CompositeAction _executableAction;

        /// <summary>
        /// Creates a new instance of <see cref="ActionBuilder{TAction, TResult}"/>
        /// </summary>
        /// <param name="action">The current action</param>
        public ActionBuilder(TAction action) : this(action, new CompositeActionForBuilder(ImmutableArray<IAction<Unit>>.Empty), action?.Name ?? "")
        {
        }

        internal ActionBuilder(TAction currentAction, CompositeAction executableAction, string name)
        {
            Action = currentAction ?? throw new ArgumentNullException(nameof(currentAction));
            _executableAction = executableAction;
            Name = name;
        }

        /// <summary>
        /// Gets the current action
        /// </summary>
        public TAction Action { get; }

        IActionBuilder<TNextAction, TNextResult> IActionBuilder.Then<TNextAction, TNextResult>(TNextAction nextAction)
        {
            if (nextAction == null)
            {
                throw new ArgumentNullException(nameof(nextAction));
            }

            var actions = new CompositeActionForBuilder(_executableAction.Actions.Add(Action.AsActionUnit()));
            var name = Name + ", Then " + nextAction.Name;
            return new ActionBuilder<TNextAction, TNextResult>(nextAction, actions, name);
        }

        /// <summary>
        /// Gets the action name
        /// </summary>
        public string Name { get; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TResult ExecuteGivenAs(IActor actor)
        {
            if (_executableAction.Actions.IsEmpty)
            {
                return Action.ExecuteGivenAs(actor);
            }
            else
            {
                _executableAction.ExecuteGivenAs(actor);
                return actor.Execute(Action);
            }
        }

        public TResult ExecuteWhenAs(IActor actor)
        {
            if (_executableAction.Actions.IsEmpty)
            {
                return Action.ExecuteWhenAs(actor);
            }
            else
            {
                _executableAction.ExecuteWhenAs(actor);
                return actor.Execute(Action);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        IActionBuilderWithPreviousResult<TNextAction, TNextResult, TAction, TResult> IActionBuilderWithCurrentAction<TAction, TResult>.Then<TNextAction, TNextResult>(Func<ActionResult<TAction, TResult>, TNextAction> nextAction)
        {
            return new ActionBuilderWithPreviousResult<TNextAction, TNextResult, TAction, TResult>(
                _ => Action,
                _executableAction,
                nextAction,
                Name);
        }

        IActionBuilder<TAction, TResult> IActionBuilder<TAction, TResult>.Named(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new ActionBuilder<TAction, TResult>(Action, _executableAction, name);
        }
    }
}
