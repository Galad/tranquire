using System;
using System.Threading.Tasks;

namespace Tranquire.Extensions;

/// <summary>
/// Represents an action which asynchronous result is transform by the given selector function
/// </summary>
/// <typeparam name="TSource">The result type of the source action</typeparam>
/// <typeparam name="TResult">The result type of the selector function</typeparam>
public sealed class SelectActionAsync<TSource, TResult> : ActionBase<Task<TResult>>
{
    /// <summary>
    /// Gets the question injected in the constructor
    /// </summary>
    public Func<TSource, Task<TResult>> Selector { get; }
    /// <summary>
    /// Gets the question injected in the constructor
    /// </summary>
    public IAction<Task<TSource>> Action { get; }


    /// <summary>Record Constructor</summary>
    /// <param name="action">The question to get the result from</param>
    /// <param name="selector">The function to apply of the question result.</param>
    public SelectActionAsync(IAction<Task<TSource>> action, Func<TSource, Task<TResult>> selector)
    {
        Selector = selector ?? throw new ArgumentNullException(nameof(selector));
        Action = action ?? throw new ArgumentNullException(nameof(action));
    }

    /// <inheritsdoc />
    public override string Name => "[Select] " + Action.Name;

    /// <inheritsdoc />
    protected override async Task<TResult> ExecuteWhen(IActor actor)
    {
        var result = await actor.Execute(Action);
        var projectedResult = await Selector(result);
        return projectedResult;
    }
}
