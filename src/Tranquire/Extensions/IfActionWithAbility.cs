using System;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Allows to execute an action only if the result of the predicate is true
    /// </summary>
    /// <typeparam name="T">The return type of the action</typeparam>
    /// <typeparam name="TGivenAbility">The action's Given ability type</typeparam>
    /// <typeparam name="TWhenAbility">The action's When ability type</typeparam>
    public sealed class IfActionWithAbility<TGivenAbility, TWhenAbility, T> : Action<T>
    {
        /// <summary>
        /// Gets the action to execute if the predicate is true
        /// </summary>
        public IAction<TGivenAbility, TWhenAbility, T> Action { get; }
        /// <summary>
        /// Gets the predicate
        /// </summary>
        public Func<bool> Predicate { get; }
        /// <summary>
        /// Gets the value which is returned by the action when the predicate is false
        /// </summary>
        public T DefaultValue { get; }

        /// <summary>
        /// Creates a new instance of <see cref="IfActionWithAbility{TGivenAbility, TWhenAbility, T}"/>
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>
        /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
        public IfActionWithAbility(Func<bool> predicate, IAction<TGivenAbility, TWhenAbility, T> action, T defaultValue)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (action == null) throw new ArgumentNullException(nameof(action));
            if ((defaultValue as object) == null) throw new ArgumentNullException(nameof(defaultValue));
            Action = action;
            Predicate = predicate;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// The action's name
        /// </summary>
        public override string Name => "[If] " + Action.Name;

        /// <summary>
        /// Execute when
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        protected override T ExecuteWhen(IActor actor)
        {            
            if (Predicate())
            {
                return actor.ExecuteWithAbility(Action);
            }
            return DefaultValue;
        }
    }

    /// <summary>
    /// Allows to execute an action only if the result of the predicate is true
    /// </summary>    
    /// <typeparam name="T">The return type of the action</typeparam>
    /// <typeparam name="TGivenAbility">The action's Given ability type</typeparam>
    /// <typeparam name="TWhenAbility">The action's When ability type</typeparam>
    /// <typeparam name="TPredicateAbility">The predicate ability type</typeparam>
    public sealed class IfActionWithAbility<TPredicateAbility, TGivenAbility, TWhenAbility, T> : Tranquire.Action<TPredicateAbility, T>
    {
        /// <summary>
        /// Gets the action to execute if the predicate is true
        /// </summary>
        public IAction<TGivenAbility, TWhenAbility, T> Action { get; }
        /// <summary>
        /// Gets the predicate
        /// </summary>
        public Func<TPredicateAbility, bool> Predicate { get; }
        /// <summary>
        /// Gets the value which is returned by the action when the predicate is false
        /// </summary>
        public T DefaultValue { get; }

        /// <summary>
        /// The action's name
        /// </summary>
        public override string Name => "[If] " + Action.Name;

        /// <summary>
        /// Creates a new instance of <see cref="IfActionWithAbility{TPredicateAbility, TGivenAbility, TWhenAbility, T}"/>
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>
        /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
        public IfActionWithAbility(Func<TPredicateAbility, bool> predicate, IAction<TGivenAbility, TWhenAbility, T> action, T defaultValue)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (action == null) throw new ArgumentNullException(nameof(action));
            if ((defaultValue as object) == null) throw new ArgumentNullException(nameof(defaultValue));
            Action = action;
            Predicate = predicate;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Execute when
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        protected override T ExecuteWhen(IActor actor, TPredicateAbility ability)
        {            
            if (Predicate(ability))
            {
                return actor.ExecuteWithAbility(Action);
            }
            return DefaultValue;
        }
    }
}
