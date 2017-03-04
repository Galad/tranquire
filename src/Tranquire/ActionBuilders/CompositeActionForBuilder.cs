using System.Collections.Immutable;

namespace Tranquire.ActionBuilders
{
    /// <summary>
    /// A composite action used by the action builders
    /// </summary>
    internal class CompositeActionForBuilder : CompositeAction
    {
        public CompositeActionForBuilder(ImmutableArray<IAction<Unit>> actions)
            : base(actions)
        {
        }

        public override string Name => string.Empty;
    }
}
