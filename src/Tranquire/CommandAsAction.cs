using System;

namespace Tranquire
{
    internal class WhenCommandAsAction<TResult> : IAction<TResult>
    {
        public IWhenCommand<TResult> Command { get; }

        public string Name => Command.Name;

        public WhenCommandAsAction(IWhenCommand<TResult> command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
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

    internal class GivenCommandAsAction<TResult> : IAction<TResult>
    {
        public IGivenCommand<TResult> Command { get; }
        public string Name => Command.Name;

        public GivenCommandAsAction(IGivenCommand<TResult> command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
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
}
