using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>
        /// <typeparam name="T">The type returned by the action</typeparam>
        /// <typeparam name="TActionAbility">The action's ability type</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>
        /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
        /// <returns>Returns a instance of <see cref="IfActionWithAbility{TAbility, T}"/></returns>        
        public static IfActionWithAbility<TActionAbility, T> If<T, TActionAbility>(this IAction<TActionAbility, TActionAbility, T> action, Func<bool> predicate, T defaultValue)
        {
            return new IfActionWithAbility<TActionAbility, T>(predicate, action, defaultValue);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>        
        /// <typeparam name="TActionAbility">The action's ability type</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>        
        /// <returns>Returns a instance of <see cref="IfActionWithAbility{TAbility, T}"/></returns>
        public static IfActionWithAbility<TActionAbility, Unit> If<TActionAbility>(this IAction<TActionAbility, TActionAbility, Unit> action, Func<bool> predicate)
        {
            return action.If(predicate, Unit.Default);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>
        /// <typeparam name="T">The type returned by the action</typeparam>
        /// <typeparam name="TActionAbility">The action's ability type</typeparam>
        /// <typeparam name="TPredicateAbility">The predicates's ability type</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>
        /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
        /// <returns>Returns a instance of <see cref="IfActionWithAbility{TPredicateAbility, TAbility, T}"/></returns>      
        public static IfActionWithAbility<TPredicateAbility, TActionAbility, T> If<T, TActionAbility, TPredicateAbility>(
            this IAction<TActionAbility, TActionAbility, T> action,
            Func<TPredicateAbility, bool> predicate,
            T defaultValue)
        {
            return new IfActionWithAbility<TPredicateAbility, TActionAbility, T>(predicate, action, defaultValue);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>        
        /// <typeparam name="TActionAbility">The action's ability type</typeparam>
        /// <typeparam name="TPredicateAbility">The predicates's ability type</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>        
        /// <returns>Returns a instance of <see cref="IfActionWithAbility{TPredicateAbility, TAbility, T}"/></returns>
        public static IfActionWithAbility<TPredicateAbility, TActionAbility, Unit> If<TActionAbility, TPredicateAbility>(
            this IAction<TActionAbility, TActionAbility, Unit> action,
            Func<TPredicateAbility, bool> predicate)
        {
            return action.If(predicate, Unit.Default);
        }
        #endregion

        /// <summary>
        /// Transform an action with requiring an ability to an action that does not requires an ability
        /// </summary>
        /// <typeparam name="TGivenAbility">Given ability</typeparam>
        /// <typeparam name="TWhenAbility">When abiltiy</typeparam>
        /// <typeparam name="TResult">The action's result</typeparam>
        /// <param name="action">The action to transform</param>
        /// <returns>A <see cref="IAction{TResult}"/> instance that hides the <paramref name="action"/></returns>
        public static IAction<TResult> AsActionWithoutAbility<TGivenAbility, TWhenAbility, TResult>(this IAction<TGivenAbility, TWhenAbility, TResult> action)
        {
            return new ActionWithAbilityToActionAdapter<TGivenAbility, TWhenAbility, TResult>(action);
        }
    }
}
