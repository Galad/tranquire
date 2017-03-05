using System;
using System.Collections.Immutable;
using System.Linq;

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
        public ActionBuilder(TAction action) : this(action, new CompositeActionForBuilder(ImmutableArray<IAction<Unit>>.Empty))
        {
        }

        internal ActionBuilder(TAction currentAction, CompositeAction executableAction)
        {
            if (currentAction == null) throw new ArgumentNullException(nameof(currentAction));
            if (executableAction == null) throw new ArgumentNullException(nameof(executableAction));
            Action = currentAction;
            _executableAction = executableAction;
        }

        /// <summary>
        /// Gets the current action
        /// </summary>
        public TAction Action { get; }

        IActionBuilder<TNextAction, TNextResult> IActionBuilder.Then<TNextAction, TNextResult>(TNextAction nextAction)            
        {
            if (nextAction == null) throw new ArgumentNullException(nameof(nextAction));
            var actions = new CompositeActionForBuilder(_executableAction.Actions.Add(Action.AsActionUnit()));
            return new ActionBuilder<TNextAction, TNextResult>(nextAction, actions);
        }

        /// <summary>
        /// Gets the action name
        /// </summary>
        public string Name
        {
            get
            {
                if (_executableAction.Actions.IsEmpty)
                {
                    return Action.Name;
                }
                return string.Join(", Then ", _executableAction.Select(a => a.Name)) + ", Then " + Action.Name;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TResult ExecuteGivenAs(IActor actor)
        {
            _executableAction.ExecuteGivenAs(actor);
            return actor.Execute(Action);
        }

        public TResult ExecuteWhenAs(IActor actor)
        {
            _executableAction.ExecuteWhenAs(actor);
            return actor.Execute(Action);
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
    }
}
