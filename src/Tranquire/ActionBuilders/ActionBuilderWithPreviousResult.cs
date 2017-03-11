using System;
using System.Collections.Immutable;

namespace Tranquire.ActionBuilders
{
    /// <summary>
    /// An action builder that gets its current action from an action factory
    /// </summary>
    /// <typeparam name="TAction">The action type</typeparam>
    /// <typeparam name="TResult">The action result type</typeparam>
    /// <typeparam name="TPreviousAction">The previous action type</typeparam>
    /// <typeparam name="TPreviousResult">The previous action result type</typeparam>
    public sealed class ActionBuilderWithPreviousResult<TAction, TResult, TPreviousAction, TPreviousResult> : IActionBuilderWithPreviousResult<TAction, TResult, TPreviousAction, TPreviousResult>
        where TAction : class, IAction<TResult>
        where TPreviousAction : class, IAction<TPreviousResult>
    {
        /// <summary>
        /// Gets the action factory
        /// </summary>
        public Func<ActionResult<TPreviousAction, TPreviousResult>, TAction> ActionFactory { get; }
        /// <summary>
        /// Gets the previous action
        /// </summary>
        public Func<IActor, TPreviousAction> PreviousAction { get; }

        /// <summary>
        /// Gets the current action name
        /// </summary>
        public string Name
        {
            get
            {
                if (_name != "")
                {
                    return _name;
                }
                return PreviousActionName + ", Then " + typeof(TAction).Name;
            }
        }

        /// <summary>
        /// Gets the previous action name
        /// </summary>
        public string PreviousActionName { get; }

        private readonly CompositeAction _executableAction;
        private readonly string _name;

        /// <summary>
        /// Creates a new instance of <see cref="ActionBuilderWithPreviousResult{TAction, TResult, TPreviousAction, TPReviousResult}"/>
        /// </summary>
        /// <param name="previousAction">A function that returns the previous action</param>
        /// <param name="executableAction">The previous executable action</param>
        /// <param name="actionFactory">The action factory</param>
        /// <param name="previousActionName">The previous action name</param>
        public ActionBuilderWithPreviousResult(
            Func<IActor, TPreviousAction> previousAction,
            CompositeAction executableAction,
            Func<ActionResult<TPreviousAction, TPreviousResult>, TAction> actionFactory,
            string previousActionName) : this(previousAction, executableAction, actionFactory, previousActionName, "")
        {
        }

        private ActionBuilderWithPreviousResult(
            Func<IActor, TPreviousAction> previousAction,
            CompositeAction executableAction,
            Func<ActionResult<TPreviousAction, TPreviousResult>, TAction> actionFactory,
            string previousActionName,
            string name)
        {
            if (actionFactory == null) throw new ArgumentNullException(nameof(actionFactory));
            if (previousAction == null) throw new ArgumentNullException(nameof(previousAction));
            if (executableAction == null) throw new ArgumentNullException(nameof(executableAction));

            ActionFactory = actionFactory;
            PreviousAction = previousAction;
            PreviousActionName = previousActionName;
            _executableAction = executableAction;
            _name = name;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TResult ExecuteGivenAs(IActor actor)
        {
            _executableAction.ExecuteGivenAs(actor);
            return Execute(actor);
        }

        public TResult ExecuteWhenAs(IActor actor)
        {
            _executableAction.ExecuteWhenAs(actor);
            return Execute(actor);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private TResult Execute(IActor actor)
        {
            return actor.Execute(GetPreviousAction(actor));
        }

        private TAction GetPreviousAction(IActor actor)
        {
            var previousAction = PreviousAction(actor);
            var result = actor.Execute(previousAction);
            var actionResult = new ActionResult<TPreviousAction, TPreviousResult>(previousAction, result);
            return ActionFactory(actionResult);
        }

        IActionBuilderWithPreviousResult<TNextAction, TNextResult, TAction, TResult> IActionBuilderWithCurrentAction<TAction, TResult>.Then<TNextAction, TNextResult>(Func<ActionResult<TAction, TResult>, TNextAction> nextAction)
        {
            return new ActionBuilderWithPreviousResult<TNextAction, TNextResult, TAction, TResult>(GetPreviousAction, _executableAction, nextAction, Name);
        }

        IActionBuilder<TNextAction, TNextResult> IActionBuilder.Then<TNextAction, TNextResult>(TNextAction nextAction)
        {
            return new ActionBuilder<TNextAction, TNextResult>(
                nextAction,
                new CompositeActionForBuilder(_executableAction.Actions.Add(new LazyAction<TAction, TResult>(GetPreviousAction, Name).AsActionUnit())),
                Name + ", Then " + nextAction.Name);
        }

        IActionBuilderWithPreviousResult<TAction, TResult, TPreviousAction, TPreviousResult> IActionBuilderWithPreviousResult<TAction, TResult, TPreviousAction, TPreviousResult>.Named(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            return new ActionBuilderWithPreviousResult<TAction, TResult, TPreviousAction, TPreviousResult>(
                PreviousAction,
                _executableAction,
                ActionFactory,
                PreviousActionName,
                name);
        }

        private class LazyAction<TLazyAction, TLazyResult> : IAction<TLazyResult> where TLazyAction : class, IAction<TLazyResult>
        {
            private readonly Func<IActor, TLazyAction> _actionFactory;

            public LazyAction(Func<IActor, TLazyAction> actionFactory, string name)
            {
                _actionFactory = actionFactory;
                Name = name;
            }

            public string Name { get; }

            public TLazyResult ExecuteGivenAs(IActor actor)
            {
                var action = _actionFactory(actor);
                return actor.Execute(action);
            }

            public TLazyResult ExecuteWhenAs(IActor actor)
            {
                var action = _actionFactory(actor);
                return actor.Execute(action);
            }
        }
    }
}
