using System;

namespace Tranquire
{
    /// <summary>
    /// Represent an action on the system
    /// </summary>
    /// <typeparam name="T">The ability required for the contexts</typeparam>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public abstract class Action<T, TResult> : IAction<T, T, TResult>
    {
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        public TResult ExecuteGivenAs(IActor actor, T ability)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(ability, nameof(ability));
            return ExecuteGiven(actor, ability);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        public TResult ExecuteWhenAs(IActor actor, T ability)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(ability, nameof(ability));
            return ExecuteWhen(actor, ability);
        }

        /// <summary>
        /// Executes the action when the method when the execution is in a When context, or when no ExecuteGiven override has been provided
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected abstract TResult ExecuteWhen(IActor actor, T ability);

        /// <summary>
        /// Executes the action when the method when the execution is in a Given context.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected virtual TResult ExecuteGiven(IActor actor, T ability)
        {
            return ExecuteWhen(actor, ability);
        }

        /// <summary>
        /// Gets the action's name
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Returns the action's <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;

        /// <summary>
        /// Execute the action in the Given context
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public TResult ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return actor.ExecuteWithAbility(this as IAction<T, T, TResult>);
        }

        /// <summary>
        /// Execute the action in the When context
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public TResult ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return actor.ExecuteWithAbility(this as IAction<T, T, TResult>);
        }
    }

    /// <summary>
    /// Represent an action on the system
    /// </summary>    
    /// <typeparam name="TGiven">The ability required for the Given context</typeparam>
    /// <typeparam name="TWhen">The ability required for the When context</typeparam>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public abstract class Action<TGiven, TWhen, TResult> : IAction<TGiven, TWhen, TResult>
    {
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor">The actor used to execute the action.</param>
        /// <param name="ability">The ability for the current action.</param>
        public TResult ExecuteGivenAs(IActor actor, TGiven ability)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(ability, nameof(ability));
            return ExecuteGiven(actor, ability);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor">The actor used to execute the action.</param>
        /// <param name="ability">The ability for the current action.</param>
        public TResult ExecuteWhenAs(IActor actor, TWhen ability)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(ability, nameof(ability));
            return ExecuteWhen(actor, ability);
        }

        /// <summary>
        /// Executes the action when the method when the execution is in a When context
        /// </summary>
        /// <param name="actor">The actor used to execute the action. Cannot be null.</param>
        /// <param name="ability">The ability for the current action. Cannot be null.</param>
        protected abstract TResult ExecuteWhen(IActor actor, TWhen ability);

        /// <summary>
        /// Executes the action when the method when the execution is in a Given context.
        /// </summary>
        /// <param name="actor">The actor used to execute the action. Cannot be null.</param>
        /// <param name="ability">The ability for the current action. Cannot be null.</param>
        protected abstract TResult ExecuteGiven(IActor actor, TGiven ability);

        /// <summary>
        /// Gets the action's name
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Returns the action's <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;

        /// <summary>
        /// Execute the action in the Given context
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public TResult ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return actor.ExecuteWithAbility(this as IAction<TGiven, TWhen, TResult>);
        }

        /// <summary>
        /// Execute the action in the When context
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public TResult ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return actor.ExecuteWithAbility(this as IAction<TGiven, TWhen, TResult>);
        }
    }

    /// <summary>
    /// Represent an action on the system
    /// </summary>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public abstract class Action<TResult> : IAction<TResult>
    {
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return ExecuteGiven(actor);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
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
        /// Gets the action's name
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Returns the action's <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }
}
