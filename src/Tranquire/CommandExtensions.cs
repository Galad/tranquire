using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{

    internal static class CommandExtensions
    {
        public static IAction<TResult> AsAction<TResult>(this IWhenCommand<TResult> command) => new WhenCommandAsAction<TResult>(command);
        public static IAction<Unit, TWhen, TResult> AsAction<TWhen, TResult>(this IWhenCommand<TWhen, TResult> command) => new WhenCommandAsAction<TWhen, TResult>(command);
        public static IAction<TResult> AsAction<TResult>(this IGivenCommand<TResult> command) => new GivenCommandAsAction<TResult>(command);
        public static IAction<TGiven, Unit, TResult> AsAction<TGiven, TResult>(this IGivenCommand<TGiven, TResult> command) => new GivenCommandAsAction<TGiven, TResult>(command);
    }
}
