using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    internal class WhenCommandAsAction<TResult> : IAction<TResult>
    {
        public IWhenCommand<TResult> Command { get; }

        public string Name => Command.ToString();

        public WhenCommandAsAction(IWhenCommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            Command = command;
        }

        public TResult ExecuteGivenAs(IActor actor)
        {
            throw new InvalidOperationException("Cannot call ExecuteGivenAs with a WhenCommand when");
        }

        public TResult ExecuteWhenAs(IActor actor)
        {
            return Command.ExecuteWhenAs(actor);
        }
    }

    internal class WhenCommandAsAction<TWhen, TResult> : IAction<Unit, TWhen, TResult>
    {
        public IWhenCommand<TWhen, TResult> Command { get; }
        public string Name => Command.ToString();

        public WhenCommandAsAction(IWhenCommand<TWhen, TResult> command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            Command = command;
        }

        public TResult ExecuteGivenAs(IActor actor, Unit ability)
        {
            throw new InvalidOperationException("Cannot call ExecuteGivenAs with a WhenCommand when");
        }

        public TResult ExecuteWhenAs(IActor actor, TWhen ability)
        {
            return Command.ExecuteWhenAs(actor, ability);
        }
    }

    internal class GivenCommandAsAction<TResult> : IAction<TResult>
    {
        public IGivenCommand<TResult> Command { get; }
        public string Name => Command.ToString();

        public GivenCommandAsAction(IGivenCommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            Command = command;
        }

        public TResult ExecuteGivenAs(IActor actor)
        {
            return Command.ExecuteGivenAs(actor);
        }

        public TResult ExecuteWhenAs(IActor actor)
        {
            throw new InvalidOperationException("Cannot call ExecuteWhenAs with a GivenCommand when");
        }
    }

    internal class GivenCommandAsAction<TGiven, TResult> : IAction<TGiven, Unit, TResult>
    {
        public IGivenCommand<TGiven, TResult> Command { get; }
        public string Name => Command.ToString();

        public GivenCommandAsAction(IGivenCommand<TGiven, TResult> command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            Command = command;
        }

        public TResult ExecuteGivenAs(IActor actor, TGiven ability)
        {
            return Command.ExecuteGivenAs(actor, ability);
        }

        public TResult ExecuteWhenAs(IActor actor, Unit ability)
        {
            throw new InvalidOperationException("Cannot call ExecuteWhenAs with a GivenCommand when");
        }
    }

    internal static class CommandExtensions
    {
        public static IAction<TResult> AsAction<TResult>(this IWhenCommand<TResult> command) => new WhenCommandAsAction<TResult>(command);
        public static IAction<Unit, TWhen, TResult> AsAction<TWhen, TResult>(this IWhenCommand<TWhen, TResult> command) => new WhenCommandAsAction<TWhen, TResult>(command);
        public static IAction<TResult> AsAction<TResult>(this IGivenCommand<TResult> command) => new GivenCommandAsAction<TResult>(command);
        public static IAction<TGiven, Unit, TResult> AsAction<TGiven, TResult>(this IGivenCommand<TGiven, TResult> command) => new GivenCommandAsAction<TGiven, TResult>(command);
    }
}
