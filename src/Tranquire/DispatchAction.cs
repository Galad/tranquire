using System;

namespace Tranquire
{
    /// <summary>
    /// Represent an action that use the <see cref="GivenAction"/> when it is called with <see cref="Tranquire.ActionBase{TResult}.ExecuteGivenAs(IActor)"/>, and <see cref="WhenAction"/> when it is called with <see cref="Tranquire.ActionBase{TResult}.ExecuteWhenAs(IActor)"/>
    /// </summary>
    public class DispatchAction<T> : ActionBase<T>
    {
        /// <inheritdoc />
        public override string Name { get; }
        /// <summary>
        /// Gets the action that is executed when <see cref="Tranquire.ActionBase{TResult}.ExecuteWhenAs(IActor)"/> is called
        /// </summary>
        public IAction<T> WhenAction { get; }
        /// <summary>
        /// Gets the action that is executed when <see cref="Tranquire.ActionBase{TResult}.ExecuteGivenAs(IActor)"/> is called
        /// </summary>
        public IAction<T> GivenAction { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DispatchAction{T}"/>
        /// </summary>
        /// <param name="name">The action name</param>
        /// <param name="givenAction">The action that is executed when the <see cref="Tranquire.ActionBase{TResult}.ExecuteGivenAs(IActor)"/> is called on the returned action</param>
        /// <param name="whenAction">The action that is executed when the <see cref="Tranquire.ActionBase{TResult}.ExecuteWhenAs(IActor)"/> is called on the returned action</param>
        public DispatchAction(string name, IAction<T> givenAction, IAction<T> whenAction)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            Name = name;
            GivenAction = givenAction ?? throw new ArgumentNullException(nameof(givenAction));
            WhenAction = whenAction ?? throw new ArgumentNullException(nameof(whenAction));
        }

        /// <inheritdoc />
        protected override T ExecuteGiven(IActor actor)
        {
            return actor.Execute(GivenAction);
        }

        /// <inheritdoc />
        protected override T ExecuteWhen(IActor actor)
        {
            return actor.Execute(WhenAction);
        }
    }

    /// <summary>
    /// A <see cref="DispatchAction{T}"/> instance that does not return any value
    /// </summary>
    public sealed class DispatchActionUnit : DispatchAction<Unit>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DispatchActionUnit"/>
        /// </summary>
        /// <param name="name">The action name</param>
        /// <param name="givenAction">The action that is executed when the <see cref="Tranquire.ActionBase{TResult}.ExecuteGivenAs(IActor)"/> is called on the returned action</param>
        /// <param name="whenAction">The action that is executed when the <see cref="Tranquire.ActionBase{TResult}.ExecuteWhenAs(IActor)"/> is called on the returned action</param>
        public DispatchActionUnit(string name, IAction<Unit> givenAction, IAction<Unit> whenAction) : base(name, givenAction, whenAction)
        {
        }
    }
}
