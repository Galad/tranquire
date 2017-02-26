using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an action that executes another action prior to the current one.
    /// </summary>
    /// <typeparam name="TPrevious">The result type of the previous action</typeparam>
    /// <typeparam name="TResult">The result type of the current action</typeparam>
    public abstract class ChainedAction<TPrevious, TResult> : IAction<TResult>
    {
        /// <summary>
        /// Gets the previous action
        /// </summary>
        public IAction<TPrevious> PreviousAction { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ChainedAction{TPrevious, TResult}"/>
        /// </summary>
        /// <param name="previousAction">The previous action</param>
        protected ChainedAction(IAction<TPrevious> previousAction)
        {
            if (previousAction == null) throw new ArgumentNullException(nameof(previousAction));
            PreviousAction = previousAction;
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            var previousResult = actor.Execute(PreviousAction);
            return ExecuteGiven(actor, previousResult);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            var previousResult = actor.Execute(PreviousAction);
            return ExecuteWhen(actor, previousResult);
        }

        /// <summary>
        /// Executes the action when the method when the execution is in a When context, or when no ExecuteGiven override has been provided
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="previousActionResult">The result of the previous action</param>
        protected abstract TResult ExecuteWhen(IActor actor, TPrevious previousActionResult);

        /// <summary>
        /// Executes the action when the method when the execution is in a Given context.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="previousActionResult">The result of the previous action</param>
        protected virtual TResult ExecuteGiven(IActor actor, TPrevious previousActionResult)
        {
            return ExecuteWhen(actor, previousActionResult);
        }

        /// <summary>
        /// Gets the previous action's name
        /// </summary>
        public string Name => $"{PreviousAction.Name}, Then {CurrentActionName}";

        /// <summary>
        /// Gets the name of the current action
        /// </summary>
        public abstract string CurrentActionName { get; }

        /// <summary>
        /// Returns the action's <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }

    /// <summary>
    /// Represent an action that executes another action returning no result prior to the current one
    /// </summary>    
    /// <typeparam name="TResult">The result type of the current action</typeparam>
    public abstract class ChainedAction<TResult> : IAction<TResult>
    {
        /// <summary>
        /// Gets the previous action
        /// </summary>
        public IAction<Unit> PreviousAction { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ChainedAction{TPrevious, TResult}"/>
        /// </summary>
        /// <param name="previousAction">The previous action</param>
        protected ChainedAction(IAction<Unit> previousAction)
        {
            if (previousAction == null) throw new ArgumentNullException(nameof(previousAction));
            PreviousAction = previousAction;
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            var previousResult = actor.Execute(PreviousAction);
            return ExecuteGiven(actor);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            var previousResult = actor.Execute(PreviousAction);
            return ExecuteWhen(actor);
        }

        /// <summary>
        /// Executes the action when the method when the execution is in a When context, or when no ExecuteGiven override has been provided
        /// </summary>
        /// <param name="actor"></param>        
        protected abstract TResult ExecuteWhen(IActor actor);

        /// <summary>
        /// Executes the action when the method when the execution is in a Given context.
        /// </summary>
        /// <param name="actor"></param>        
        protected virtual TResult ExecuteGiven(IActor actor)
        {
            return ExecuteWhen(actor);
        }

        /// <summary>
        /// Gets the previous action's name
        /// </summary>
        public string Name => $"{PreviousAction.Name}, Then {CurrentActionName}";

        /// <summary>
        /// Gets the name of the current action
        /// </summary>
        public abstract string CurrentActionName { get; }

        /// <summary>
        /// Returns the action's <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }
}
