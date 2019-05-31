using System;
using System.Threading.Tasks;
using Tranquire.Extensions;
using SM = Tranquire.Extensions.SelectMany;

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

        private sealed class DiscardActionResult<TResult> : ActionBaseUnit
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
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<TResult> SelectMany<TSource, TResult>(this IAction<TSource> source, Func<TSource, IAction<TResult>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<TSource>(), selector, SM.Execute<TResult>());
            return selectMany.ToAction();
        }

        /// <summary>
        /// Projects the result of an action into a new question.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IQuestion<TResult> SelectMany<TSource, TResult>(this IAction<TSource> source, Func<TSource, IQuestion<TResult>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<TSource>(), selector, SM.AsksFor<TResult>());
            return selectMany.ToQuestion();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new action.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<Task<TResult>> SelectMany<TSource, TResult>(this IAction<Task<TSource>> source, Func<TSource, IAction<TResult>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task<TSource>>(), selector, SM.ExecuteAsync<TResult>());
            return selectMany.ToAction();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new asynchronous action.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<Task<TResult>> SelectMany<TSource, TResult>(this IAction<Task<TSource>> source, Func<TSource, IAction<Task<TResult>>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task<TSource>>(), selector, SM.ExecuteAsync<Task<TResult>>());
            return selectMany.ToAction();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new action.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<Task> SelectMany<TSource>(this IAction<Task<TSource>> source, Func<TSource, IAction<Task>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task<TSource>>(), selector, SM.ExecuteAsync<Task>());
            return selectMany.ToAction();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new question
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IQuestion<Task<TResult>> SelectMany<TSource, TResult>(this IAction<Task<TSource>> source, Func<TSource, IQuestion<TResult>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task<TSource>>(), selector, SM.AsksForAsync<TResult>());
            return selectMany.ToQuestion();
        }

        /// <summary>
        /// Projects the result of an action into a new asynchronous question.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IQuestion<Task<TResult>> SelectMany<TSource, TResult>(this IAction<Task<TSource>> source, Func<TSource, IQuestion<Task<TResult>>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task<TSource>>(), selector, SM.AsksForAsync<Task<TResult>>());
            return selectMany.ToQuestion();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new action.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<Task<TResult>> SelectMany<TResult>(this IAction<Task> source, Func<IAction<TResult>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task>(), selector, SM.ExecuteAsync<TResult>());
            return selectMany.ToAction();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new action.
        /// </summary>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<Task> SelectMany(this IAction<Task> source, Func<IAction<Task>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task>(), selector, SM.ExecuteAsync<Task>());
            return selectMany.ToAction();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new action.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<Task<TResult>> SelectMany<TResult>(this IAction<Task> source, Func<IAction<Task<TResult>>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task>(), selector, SM.ExecuteAsync<Task<TResult>>());
            return selectMany.ToAction();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new question.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IQuestion<Task<TResult>> SelectMany<TResult>(this IAction<Task> source, Func<IQuestion<TResult>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task>(), selector, SM.AsksForAsync<TResult>());
            return selectMany.ToQuestion();
        }

        /// <summary>
        /// Projects the result of an asynchronous action into a new question.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The action which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IQuestion<Task<TResult>> SelectMany<TResult>(this IAction<Task> source, Func<IQuestion<Task<TResult>>> selector)
        {
            var selectMany = SM.Create(source, SM.Execute<Task>(), selector, SM.AsksForAsync<Task<TResult>>());
            return selectMany.ToQuestion();
        }
        #endregion

        #region Select
        /// <summary>
        /// Projects the result of an action into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action">The action which result is transformed</param>
        /// <param name="selector">A transform function to the action result.</param>
        /// <returns></returns>
        public static IAction<TResult> Select<TSource, TResult>(this IAction<TSource> action, Func<TSource, TResult> selector)
        {
            return new SelectAction<TSource, TResult>(action, selector);
        }

        /// <summary>
        /// Projects the result of an action into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action">The action which result is transformed</param>
        /// <param name="selector">A transform function to the action result.</param>
        /// <returns></returns>
        public static IAction<Task<TResult>> Select<TSource, TResult>(this IAction<Task<TSource>> action, Func<TSource, TResult> selector)
        {
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectActionAsync<TSource, TResult>(action, s => Task.FromResult(selector(s)));
        }

        /// <summary>
        /// Projects the result of an action into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action">The action which result is transformed</param>
        /// <param name="selector">A transform function to the action result.</param>
        /// <returns></returns>
        public static IAction<Task<TResult>> Select<TSource, TResult>(this IAction<Task<TSource>> action, Func<TSource, Task<TResult>> selector)
        {
            return new SelectActionAsync<TSource, TResult>(action, selector);
        }
        #endregion

        #region Tagged
        /// <summary>
        /// Create a tagged action from the current action.
        /// </summary>
        /// <typeparam name="T">The action type</typeparam>
        /// <typeparam name="TTag">The tag type</typeparam>
        /// <param name="action">The action to tag</param>
        /// <param name="tag">The tag to apply to the action</param>
        /// <returns>An action with a single tag <paramref name="tag"/></returns>
        public static IAction<T> Tagged<T, TTag>(this IAction<T> action, TTag tag)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return Actions.CreateTagged(action.Name, (tag, action));
        }
        #endregion

        #region Named
        /// <summary>
        /// Change the named of the action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IAction<T> Named<T>(this IAction<T> action, string name)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new NamedAction<T>(action, name);
        }

        private class NamedAction<T> : IAction<T>
        {
            private readonly IAction<T> _action;

            public NamedAction(IAction<T> action, string name)
            {
                _action = action;
                Name = name;
            }

            public string Name { get; }

            public T ExecuteGivenAs(IActor actor)
            {
                return _action.ExecuteGivenAs(actor);
            }

            public T ExecuteWhenAs(IActor actor)
            {
                return _action.ExecuteWhenAs(actor);
            }
        }
        #endregion
    }
}
