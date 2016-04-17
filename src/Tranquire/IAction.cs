using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public interface IAction<TGiven, TWhen> : IGivenCommand<TGiven>, IWhenCommand<TWhen>
    {
    }

    public interface IAction : IGivenCommand, IWhenCommand
    {

    }
}
