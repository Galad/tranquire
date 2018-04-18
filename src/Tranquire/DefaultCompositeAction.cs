using System;

namespace Tranquire
{
    /// <summary>
    /// Represent a composite action that can be instanciated
    /// </summary>
    public class DefaultCompositeAction : CompositeAction
    {
        /// <summary>
        /// Creates a new instance of  <see cref="DefaultCompositeAction"/>
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <param name="actions">The actions to execute.</param>
        public DefaultCompositeAction(string name, params IAction<Unit>[] actions) : base(actions)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        /// <inheritsdoc />
        public override string Name { get; }
    }
}
