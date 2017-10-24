using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Contains utility functions for actions
    /// </summary>
    public static class Actions
    {
        /// <summary>
        /// An empty action that does nothing
        /// </summary>
        public static readonly IAction<Unit> Empty = new EmptyAction();

        private class EmptyAction : IAction<Unit>
        {
            public string Name => "Empty action";
            public Unit ExecuteGivenAs(IActor actor) => Unit.Default;
            public Unit ExecuteWhenAs(IActor actor) => Unit.Default;
            public override string ToString() => Name;
        }

        private class ResultAction<T> : IAction<T>
        {
            private readonly T _result;

            public ResultAction(T result)
            {
                _result = result;
            }

            public string Name => "Returns " + _result.ToString();

            public T ExecuteGivenAs(IActor actor)
            {
                return _result;
            }

            public T ExecuteWhenAs(IActor actor)
            {
                return _result;
            }

            public override string ToString() => Name;
        }

        /// <summary>
        /// Creates an action that returns the given value
        /// </summary>
        /// <typeparam name="TResult">The value type</typeparam>
        /// <param name="result">The value returned by the action</param>
        /// <returns></returns>
        public static IAction<TResult> FromResult<TResult>(TResult result)
        {
            return new ResultAction<TResult>(result);
        }

        private class DelegateAction : ActionUnit
        {
            public override string Name { get; }
            public System.Action<IActor> Action { get; }

            public DelegateAction(string name, System.Action<IActor> action)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                Name = name;
                this.Action = action;
            }

            protected override void ExecuteWhen(IActor actor)
            {
                Action(actor);
            }
        }

        /// <summary>
        /// Creates an action that executes the given delegate
        /// </summary>
        /// <param name="name">The action name</param>
        /// <param name="action">The delegate to execute</param>
        /// <returns></returns>
        public static IAction<Unit> Create(string name, System.Action<IActor> action)
        {
            return new DelegateAction(name, action);
        }

        private class DelegateAction<T> : ActionUnit<T>
        {
            public override string Name { get; }
            public System.Action<IActor, T> Action { get; }

            public DelegateAction(string name, System.Action<IActor, T> action)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                Name = name;
                this.Action = action;
            }

            protected override void ExecuteWhen(IActor actor, T ability)
            {
                Action(actor, ability);
            }
        }

        /// <summary>
        /// Creates an action that executes the given delegate
        /// </summary>
        /// <param name="name">The action name</param>
        /// <param name="action">The delegate to execute</param>
        /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
        public static IAction<T, T, Unit> Create<T>(string name, System.Action<IActor, T> action)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            return new DelegateAction<T>(name, action);
        }

        private class FuncAction<TResult> : Action<TResult>
        {
            public override string Name { get; }
            public Func<IActor, TResult> Action { get; }

            public FuncAction(string name, Func<IActor, TResult> action)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                Name = name;
                Action = action;
            }

            protected override TResult ExecuteWhen(IActor actor)
            {
                return Action(actor);
            }
        }

        /// <summary>
        /// Creates an action that executes the given delegate and return its value
        /// </summary>
        /// <param name="name">The action name</param>
        /// <param name="action">The delegate to execute</param>
        /// <returns></returns>
        public static IAction<T> Create<T>(string name, Func<IActor, T> action)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new FuncAction<T>(name, action);
        }

        private class FuncAction<TAbility, TResult> : Action<TAbility, TResult>
        {
            public override string Name { get; }
            public Func<IActor, TAbility, TResult> Action { get; }

            public FuncAction(string name, Func<IActor, TAbility, TResult> action)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                Name = name;
                Action = action;
            }

            protected override TResult ExecuteWhen(IActor actor, TAbility ability)
            {                
                return Action(actor, ability);
            }
        }

        /// <summary>
        /// Creates an action that executes the given delegate and return its value
        /// </summary>
        /// <param name="name">The action name</param>
        /// <param name="action">The delegate to execute</param>
        /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
        public static IAction<TAbility, TAbility, TResult> Create<TResult, TAbility>(string name, Func<IActor, TAbility, TResult> action)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new FuncAction<TAbility, TResult>(name, action);
        }
    }
}
