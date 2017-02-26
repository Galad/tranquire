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
        /// <typeparam name="TGivenAbility">The action's Given ability type</typeparam>
        /// <typeparam name="TWhenAbility">The action's When ability type</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>
        /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
        /// <returns>Returns a instance of <see cref="IfActionWithAbility{TGivenAbility, TWhenAbility, T}"/></returns>        
        public static IfActionWithAbility<TGivenAbility, TWhenAbility, T> If<T, TGivenAbility, TWhenAbility>(this IAction<TGivenAbility, TWhenAbility, T> action, Func<bool> predicate, T defaultValue)
        {
            return new IfActionWithAbility<TGivenAbility, TWhenAbility, T>(predicate, action, defaultValue);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>        
        /// <typeparam name="TGivenAbility">The action's Given ability type</typeparam>
        /// <typeparam name="TWhenAbility">The action's When ability type</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>        
        /// <returns>Returns a instance of <see cref="IfActionWithAbility{TGivenAbility, TWhenAbility, T}"/></returns>
        public static IfActionWithAbility<TGivenAbility, TWhenAbility, Unit> If<TGivenAbility, TWhenAbility>(this IAction<TGivenAbility, TWhenAbility, Unit> action, Func<bool> predicate)
        {
            return action.If(predicate, Unit.Default);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>
        /// <typeparam name="T">The type returned by the action</typeparam>
        /// <typeparam name="TGivenAbility">The action's Given ability type</typeparam>
        /// <typeparam name="TWhenAbility">The action's When ability type</typeparam>
        /// <typeparam name="TPredicateAbility">The predicates's ability type</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>
        /// <param name="defaultValue">The value which is returned by the action when the predicate is false</param>
        /// <returns>Returns a instance of <see cref="IfActionWithAbility{TPredicateAbility, TAbility, T}"/></returns>      
        public static IfActionWithAbility<TPredicateAbility, TGivenAbility, TWhenAbility, T> If<T, TGivenAbility, TWhenAbility, TPredicateAbility>(
            this IAction<TGivenAbility, TWhenAbility, T> action,
            Func<TPredicateAbility, bool> predicate,
            T defaultValue)
        {
            return new IfActionWithAbility<TPredicateAbility, TGivenAbility, TWhenAbility, T>(predicate, action, defaultValue);
        }

        /// <summary>
        /// Execute the current action only if the given predicate is true
        /// </summary>        
        /// <typeparam name="TGivenAbility">The action's Given ability type</typeparam>
        /// <typeparam name="TWhenAbility">The action's When ability type</typeparam>
        /// <typeparam name="TPredicateAbility">The predicates's ability type</typeparam>
        /// <param name="predicate">The predicate</param>
        /// <param name="action">The action to execute if the predicate is true</param>        
        /// <returns>Returns a instance of <see cref="IfActionWithAbility{TPredicateAbility, TAbility, T}"/></returns>
        public static IfActionWithAbility<TPredicateAbility, TGivenAbility, TWhenAbility, Unit> If<TGivenAbility, TWhenAbility, TPredicateAbility>(
            this IAction<TGivenAbility, TWhenAbility, Unit> action,
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
