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
    /// <typeparam name="TGiven">The ability required for the When context</typeparam>
    /// <typeparam name="TWhen">The ability required for the When context</typeparam>
    public interface IAction<TGiven, TWhen> : IGivenCommand<TGiven>, IWhenCommand<TWhen>
    {
    }

    /// <summary>
    /// Represent an executable action
    /// </summary>
    public interface IAction : IGivenCommand, IWhenCommand
    {

    }
}
