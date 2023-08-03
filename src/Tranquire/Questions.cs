using System;
using System.Collections.Immutable;
using System.Linq;

namespace Tranquire;

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

    /// <summary>
    /// Creates a question that return the given value
    /// </summary>
    /// <param name="name">The action name</param>
    /// <param name="value">The value to return</param>
    /// <returns></returns>
    public static IQuestion<TAnswer> Create<TAnswer>(string name, TAnswer value)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return new FuncQuestions<TAnswer>(name, _ => value);
    }

    private sealed class FuncQuestions<TAnswer> : QuestionBase<TAnswer>
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

    private sealed class FuncQuestions<TAbility, TAnswer> : QuestionBase<TAbility, TAnswer>
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

    private sealed class ResultQuestion<TAnswer> : QuestionBase<TAnswer>
    {
        private readonly TAnswer result;

        public ResultQuestion(TAnswer result)
        {
            this.result = result;
        }

        public override string Name => "Returns " + result.ToString();

        protected override TAnswer Answer(IActor actor) => result;
    }

    #region Tagged question
    /// <summary>
    /// Creates an action that identifies the action to use based on a tag.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTag"></typeparam>
    /// <param name="questions"></param>  
    /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
    public static IQuestion<IActionTags<TTag>, T> CreateTagged<T, TTag>(params (TTag tag, IQuestion<T> question)[] questions)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        if (questions == null)
        {
            throw new ArgumentNullException(nameof(questions));
        }

        return new TaggedQuestion<T, TTag>(null, questions.ToImmutableDictionary(t => t.tag, t => t.question));
    }

    /// <summary>
    /// Creates an action that identifies the action to use based on a tag.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTag"></typeparam>
    /// <param name="name"></param>
    /// <param name="questions"></param>      
    /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
    public static IQuestion<IActionTags<TTag>, T> CreateTagged<T, TTag>(string name, params (TTag tag, IQuestion<T> question)[] questions)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (questions == null)
        {
            throw new ArgumentNullException(nameof(questions));
        }

        return new TaggedQuestion<T, TTag>(name, questions.ToImmutableDictionary(t => t.tag, t => t.question));
    }

    /// <summary>
    /// Represents a question that can be performed in different ways, depending on the context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTag"></typeparam>
    private sealed class TaggedQuestion<T, TTag> : QuestionBase<IActionTags<TTag>, T>
    {
        public TaggedQuestion(string name, ImmutableDictionary<TTag, IQuestion<T>> questions)
        {
            Question = questions;
            Name = name ?? $"Tagged question with {string.Join(", ", Question.Keys.OrderBy(k => k))}";
        }

        public ImmutableDictionary<TTag, IQuestion<T>> Question { get; }

        public override string Name { get; }

        protected override T Answer(IActor actor, IActionTags<TTag> ability)
        {
            var bestTag = ability.FindBestWhenTag(Question.Keys);
            var action = Question[bestTag];
            return actor.AsksFor(action);
        }
    }
    #endregion
}
