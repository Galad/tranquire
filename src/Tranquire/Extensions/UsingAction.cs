using System;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents an action that creates a <see cref="IDisposable"/> and executes the provided action. The <see cref="IDisposable"/> is finally disposed.
    /// </summary>
    /// <typeparam name="T">The action return type</typeparam>    
    public sealed class UsingAction<T> : ActionBase<T>
    {
        /// <summary>
        /// Gets the action that creates the <see cref="IDisposable"/> instance that was provided in the constructor
        /// </summary>
        public IAction<IDisposable> DisposableAction { get; }
        /// <summary>
        /// Gets the action to execute that was provided in the constructor
        /// </summary>
        public IAction<T> ActionToExecute { get; }

        /// <summary>
        /// Creates a new instance of <see cref="UsingAction{T}"/>
        /// </summary>
        /// <param name="actionToExecute">The action to execute</param>
        /// <param name="disposableAction">The action that creates a <see cref="IDisposable"/> instance</param>
        public UsingAction(IAction<IDisposable> disposableAction, IAction<T> actionToExecute)
        {
            DisposableAction = disposableAction ?? throw new ArgumentNullException(nameof(disposableAction));
            ActionToExecute = actionToExecute ?? throw new ArgumentNullException(nameof(actionToExecute));
        }

        /// <inheritsdoc />
        public override string Name => $"Using {DisposableAction.Name} with {ActionToExecute.Name}";

        /// <inheritsdoc />
        protected override T ExecuteWhen(IActor actor)
        {
            using (actor.Execute(DisposableAction))
            {
                return actor.Execute(ActionToExecute);
            }
        }
    }
}
