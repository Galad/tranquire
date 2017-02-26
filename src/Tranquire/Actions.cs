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
    }
}
