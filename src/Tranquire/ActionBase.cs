namespace Tranquire
{
    /// <summary>
    /// Represent an action on the system
    /// </summary>
    /// <typeparam name="T">The ability required for the contexts</typeparam>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
#pragma warning disable CS0618 // Type or member is obsolete
    public abstract class ActionBase<T, TResult> : IAction<T, TResult>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        public TResult ExecuteGivenAs(IActor actor, T ability)
        {
            if (actor == null)
            {
                throw new System.ArgumentNullException(nameof(actor));
            }
            if (ability == null)
            {
                throw new System.ArgumentNullException(nameof(ability));
            }

            return ExecuteGiven(actor, ability);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        public TResult ExecuteWhenAs(IActor actor, T ability)
        {
            if (actor == null)
            {
                throw new System.ArgumentNullException(nameof(actor));
            }
            if (ability == null)
            {
                throw new System.ArgumentNullException(nameof(ability));
            }

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
            return Execute(actor);
        }

        /// <summary>
        /// Execute the action in the When context
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public TResult ExecuteWhenAs(IActor actor)
        {
            return Execute(actor);
        }

        private TResult Execute(IActor actor)
        {
            if (actor == null)
            {
                throw new System.ArgumentNullException(nameof(actor));
            }

#pragma warning disable CS0618 // Type or member is obsolete
            return actor.ExecuteWithAbility(this as IAction<T, TResult>);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
    
    /// <summary>
    /// Represent an action on the system
    /// </summary>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
#pragma warning disable CS0618 // Type or member is obsolete
    public abstract class ActionBase<TResult> : IAction<TResult>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteGivenAs(IActor actor)
        {
            if (actor == null)
            {
                throw new System.ArgumentNullException(nameof(actor));
            }

            return ExecuteGiven(actor);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteWhenAs(IActor actor)
        {
            if (actor == null)
            {
                throw new System.ArgumentNullException(nameof(actor));
            }

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
