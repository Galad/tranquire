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

    internal class WhenCommandAsAction<TWhen, TResult> : Action<TWhen, TResult>
    {
        public IWhenCommand<TWhen, TResult> Command { get; }
        public override string Name => Command.Name;

        public WhenCommandAsAction(IWhenCommand<TWhen, TResult> command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        protected override TResult ExecuteWhen(IActor actor, TWhen ability)
        {
            return Command.ExecuteWhenAs(actor, ability);
        }

        protected override TResult ExecuteGiven(IActor actor, TWhen ability)
        {
            throw new InvalidOperationException("Cannot call ExecuteGivenAs with a WhenCommand when");
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

    internal class GivenCommandAsAction<TGiven, TResult> : Action<TGiven, TResult>
    {
        public IGivenCommand<TGiven, TResult> Command { get; }
        public override string Name => Command.Name;

        public GivenCommandAsAction(IGivenCommand<TGiven, TResult> command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        protected override TResult ExecuteWhen(IActor actor, TGiven ability)
        {
            throw new InvalidOperationException("Cannot call ExecuteWhenAs with a GivenCommand when");
        }

        protected override TResult ExecuteGiven(IActor actor, TGiven ability)
        {
            return Command.ExecuteGivenAs(actor, ability);
        }
    }
}
