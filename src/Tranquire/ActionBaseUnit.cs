using System;

namespace Tranquire
{
#pragma warning disable CS0618 // Type or member is obsolete
    /// <summary>
    /// Represent an action on the system returning no value
    /// </summary>
    /// <typeparam name="T">The ability type. It used for both When and Given contexts</typeparam>
    public abstract class ActionBaseUnit<T> : IAction<T, Unit>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        public Unit ExecuteGivenAs(IActor actor, T ability)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (ability == null)
            {
                throw new ArgumentNullException(nameof(ability));
            }

            ExecuteGiven(actor, ability);
            return Unit.Default;
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        public Unit ExecuteWhenAs(IActor actor, T ability)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (ability == null)
            {
                throw new ArgumentNullException(nameof(ability));
            }

            ExecuteWhen(actor, ability);
            return Unit.Default;
        }

        /// <summary>
        /// Executes the action when the method when the execution is in a When context, or when no ExecuteGiven override has been provided
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected abstract void ExecuteWhen(IActor actor, T ability);

        /// <summary>
        /// Executes the action when the method when the execution is in a Given context. Overrides this method without calling base.ExecuteGiven to execute a different action in the Given context.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected virtual void ExecuteGiven(IActor actor, T ability)
        {
            ExecuteWhen(actor, ability);
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
        public Unit ExecuteGivenAs(IActor actor)
        {
            return Execute(actor);
        }

        /// <summary>
        /// Execute the action in the When context
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public Unit ExecuteWhenAs(IActor actor)
        {
            return Execute(actor);
        }

        private Unit Execute(IActor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

#pragma warning disable CS0618 // Type or member is obsolete
            return actor.ExecuteWithAbility(this as IAction<T, Unit>);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <summary>
    /// Represent an action on the system returning no value
    /// </summary>
    public abstract class ActionBaseUnit : IAction<Unit>
    {
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public Unit ExecuteGivenAs(IActor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            ExecuteGiven(actor);
            return Unit.Default;
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor"></param>
        public Unit ExecuteWhenAs(IActor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            ExecuteWhen(actor);
            return Unit.Default;
        }

        /// <summary>
        /// Executes the action when the method when the execution is in a When context, or when no ExecuteGiven override has been provided
        /// </summary>
        /// <param name="actor"></param>
        protected abstract void ExecuteWhen(IActor actor);

        /// <summary>
        /// Executes the action when the method when the execution is in a Given context.
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void ExecuteGiven(IActor actor)
        {
            ExecuteWhen(actor);
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