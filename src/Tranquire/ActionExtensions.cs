using System;
using Tranquire.Extensions;

namespace Tranquire
{
    /// <summary>
    /// Provides extension for actions
    /// </summary>
    public static class ActionExtensions
    {
        #region If
        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>
        /// <typeparam name="T">The type returned by the action</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>
        /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
        /// <returns>Returns a instance of <see cref="IfAction{T}"/></returns>
        public static IfAction<T> If<T>(this IAction<T> action, Func<bool> predicate, T defaultValue)
        {
            return new IfAction<T>(predicate, action, defaultValue);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>        
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>        
        /// <returns>Returns a instance of <see cref="IfAction{T}"/></returns>
        public static IfAction<Unit> If(this IAction<Unit> action, Func<bool> predicate)
        {
            return action.If(predicate, Unit.Default);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>
        /// <typeparam name="T">The type returned by the action</typeparam>
        /// <typeparam name="TPredicateAbility"></typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>
        /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
        /// <returns>Returns a instance of <see cref="IfAction{TAbility, T}"/></returns>
        public static IfAction<TPredicateAbility, T> If<T, TPredicateAbility>(this IAction<T> action, Func<TPredicateAbility, bool> predicate, T defaultValue)
        {
            return new IfAction<TPredicateAbility, T>(predicate, action, defaultValue);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>        
        /// <typeparam name="TPredicateAbility"></typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>        
        /// <returns>Returns a instance of <see cref="IfAction{TAbility, T}"/></returns>
        public static IfAction<TPredicateAbility, Unit> If<TPredicateAbility>(this IAction<Unit> action, Func<TPredicateAbility, bool> predicate)
        {
            return action.If(predicate, Unit.Default);
        }
        #endregion

        #region AsActionUnit
        /// <summary>
        /// Transform the given action to a action returning <see cref="Unit"/> that execute the action and discards its result
        /// </summary>
        /// <typeparam name="TResult">The action result type</typeparam>
        /// <param name="action">The action to transform</param>
        /// <returns>A new action</returns>
        public static IAction<Unit> AsActionUnit<TResult>(this IAction<TResult> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new DiscardActionResult<TResult>(action);
        }

        private sealed class DiscardActionResult<TResult> : ActionUnit
        {
            private readonly IAction<TResult> _action;

            public DiscardActionResult(IAction<TResult> action)
            {
                _action = action;
            }

            public override string Name => _action.Name;

            protected override void ExecuteWhen(IActor actor) => _action.ExecuteWhenAs(actor);
            protected override void ExecuteGiven(IActor actor) => _action.ExecuteGivenAs(actor);
        }
        #endregion

        #region Using
        /// <summary>
        /// Creates an action that executes the <paramref name="disposableAction"/>, then the <paramref name="actionToExecute"/> and finally dispose the result of the <paramref name="disposableAction"/>
        /// </summary>
        /// <typeparam name="T">The action return type</typeparam>
        /// <param name="actionToExecute">The action to execute</param>
        /// <param name="disposableAction">The action that creates a <see cref="IDisposable"/> instance</param>
        /// <returns></returns>
        public static IAction<T> Using<T>(this IAction<T> actionToExecute, IAction<IDisposable> disposableAction) => new UsingAction<T>(disposableAction, actionToExecute);
        #endregion

        #region SelectMany
        /// <summary>
        /// Projects the result of an action into a new action.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<TResult> SelectMany<TSource, TResult>(this IAction<TSource> action, Func<TSource, IAction<TResult>> selector)
        {
            return new SelectManyAction<TSource, TResult>(action, selector);
        }
        #endregion
    }
}
