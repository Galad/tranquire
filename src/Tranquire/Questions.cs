using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquire
{
    /// <summary>
    /// Contains utility functions for questions
    /// </summary>
    public static class Questions
    {
        /// <summary>
        /// Creates a question that executes the given function and returns its result
        /// </summary>
        /// <param name="name">The action name</param>
        /// <param name="func">The function to execute</param>
        /// <returns></returns>
        public static IQuestion<TAnswer> Create<TAnswer>(string name, Func<IActor, TAnswer> func)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return new FuncQuestions<TAnswer>(name, func);
        }

        private class FuncQuestions<TAnswer> : QuestionBase<TAnswer>
        {
            private readonly string name;
            private readonly Func<IActor, TAnswer> func;

            public FuncQuestions(string name, Func<IActor, TAnswer> func)
            {
                this.name = name;
                this.func = func;
            }

            public override string Name => name;

            protected override TAnswer Answer(IActor actor)
            {
                return func(actor);
            }
        }

        /// <summary>
        /// Creates a question that executes the given function and returns its result
        /// </summary>
        /// <param name="name">The action name</param>
        /// <param name="func">The function to execute</param>
        /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
        public static IQuestion<TAbility, TAnswer> Create<TAbility, TAnswer>(string name, Func<IActor, TAbility, TAnswer> func)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return new FuncQuestions<TAbility, TAnswer>(name, func);
        }

        private class FuncQuestions<TAbility, TAnswer> : QuestionBase<TAbility, TAnswer>
        {
            private readonly string name;
            private readonly Func<IActor, TAbility, TAnswer> func;

            public FuncQuestions(string name, Func<IActor, TAbility, TAnswer> func)
            {
                this.name = name;
                this.func = func;
            }

            public override string Name => name;

            protected override TAnswer Answer(IActor actor, TAbility ability)
            {
                return func(actor, ability);
            }
        }

        /// <summary>
        /// Creates a question that returns the given value
        /// </summary>
        /// <typeparam name="TAnswer">The value type</typeparam>
        /// <param name="result">The value returned by the question</param>
        /// <returns></returns>
        public static IQuestion<TAnswer> FromResult<TAnswer>(TAnswer result)
        {
            return new ResultQuestion<TAnswer>(result);
        }

        private class ResultQuestion<TAnswer> : QuestionBase<TAnswer>
        {
            private readonly TAnswer result;

            public ResultQuestion(TAnswer result)
            {
                this.result = result;
            }

            public override string Name => "Returns " + result.ToString();

            protected override TAnswer Answer(IActor actor) => result;
        }
    }
}
