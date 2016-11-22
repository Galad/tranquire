using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an executable action requiring an ability
    /// </summary>
    /// <typeparam name="TGiven">The ability required for the Given context</typeparam>
    /// <typeparam name="TWhen">The ability required for the When context</typeparam>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public interface IAction<TGiven, TWhen, out TResult> : IGivenCommand<TGiven, TResult>, IWhenCommand<TWhen, TResult>
    {
    }

    /// <summary>
    /// Represent an executable action
    /// </summary>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public interface IAction<out TResult> : IGivenCommand<TResult>, IWhenCommand<TResult>
    {
    }

    ///// <summary>
    ///// Represent an executable action
    ///// </summary>    
    //public interface IAction : IGivenCommand<Unit>, IWhenCommand<Unit>
    //{
    //}

    ///// <summary>
    ///// Represent an executable action
    ///// </summary>    
    ///// <typeparam name="TGiven">The ability required for the Given context</typeparam>
    ///// <typeparam name="TWhen">The ability required for the When context</typeparam>
    //public interface IAction<TGiven, TWhen> : IGivenCommand<TGiven, Unit>, IWhenCommand<TWhen, Unit>
    //{
    //}
}
