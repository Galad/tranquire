using System;

namespace Tranquire
{
    /// <summary>
    /// Represent an executable action requiring an ability
    /// </summary>
    /// <typeparam name="TGiven">The ability required for the Given context</typeparam>
    /// <typeparam name="TWhen">The ability required for the When context</typeparam>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    [Obsolete("Prefer using IAction<T> when exposing an action, or inheriting the abstract class IAction<TGiven, TWhen, T> when implementing one.", false)]
    public interface IAction<TGiven, TWhen, out TResult> : IGivenCommand<TGiven, TResult>, IWhenCommand<TWhen, TResult>, IAction<TResult>
    {
    }

    /// <summary>
    /// Represent an executable action
    /// </summary>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public interface IAction<out TResult> : IGivenCommand<TResult>, IWhenCommand<TResult>
    {
    }
}
