using System;
using System.Threading.Tasks;

namespace Tranquire.Extensions;

/// <summary>
/// Represents a question which asynchronous result is transform by the given selector function
/// </summary>
/// <typeparam name="TSource">The result type of the source question</typeparam>
/// <typeparam name="TResult">The result type of the selector function</typeparam>
public sealed class SelectQuestionAsync<TSource, TResult> : QuestionBase<Task<TResult>>
{
    /// <summary>
    /// Gets the question injected in the constructor
    /// </summary>
    public Func<TSource, Task<TResult>> Selector { get; }
    /// <summary>
    /// Gets the question injected in the constructor
    /// </summary>
    public IQuestion<Task<TSource>> Question { get; }


    /// <summary>Record Constructor</summary>
    /// <param name="question">The question to get the result from</param>
    /// <param name="selector">The function to apply of the question result.</param>
    public SelectQuestionAsync(IQuestion<Task<TSource>> question, Func<TSource, Task<TResult>> selector)
    {
        Selector = selector ?? throw new ArgumentNullException(nameof(selector));
        Question = question ?? throw new ArgumentNullException(nameof(question));
    }

    /// <inheritsdoc />
    public override string Name => "[Select] " + Question.Name;

    /// <inheritsdoc />
    protected override async Task<TResult> Answer(IActor actor)
    {
        var result = await actor.AsksFor(Question);
        var projectedResult = await Selector(result);
        return projectedResult;
    }
}
