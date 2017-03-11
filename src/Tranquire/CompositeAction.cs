using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Tranquire
{
    /// <summary>
    /// Represent a <see cref="IAction{TResult}"/> composed of several <see cref="IAction{TResult}"/>
    /// </summary>    
#pragma warning disable CA1710
    public abstract class CompositeAction : IAction<Unit>, IEnumerable<IAction<Unit>>
#pragma warning restore CA1710
    {
        private class EmptyCompositeAction : CompositeAction
        {
            public override string Name => "Empty composite action";
        }

        private class AnonymousCompositeAction : CompositeAction
        {
            public override string Name => "Multiple composite actions";
            public AnonymousCompositeAction(ImmutableArray<IAction<Unit>> actions) : base(actions)
            {
            }
        }

        /// <summary>
        /// Gets the actions executed by this composite action
        /// </summary>
        public ImmutableArray<IAction<Unit>> Actions { get; }
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// Creates a new instance of <see cref="CompositeAction"/>
        /// </summary>
        /// <param name="actions">The actions executed by this composite action</param>
        protected CompositeAction(ImmutableArray<IAction<Unit>> actions)
        {
            Guard.ForNull(actions, nameof(actions));
            Actions = actions;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CompositeAction"/>
        /// </summary>
        /// <param name="actions">The actions executed by this composite action</param>
        protected CompositeAction(params IAction<Unit>[] actions) : this(ImmutableArray.Create(actions))
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="CompositeAction"/> using a builder
        /// </summary>
        /// <param name="compositeActionsBuilder">A function taking an empty task and returning a task containing the actions to execute</param>
        protected CompositeAction(Func<CompositeAction, CompositeAction> compositeActionsBuilder) : this(GetActionsFromCompositeActionBuilder(compositeActionsBuilder))
        {
        }

        private static ImmutableArray<IAction<Unit>> GetActionsFromCompositeActionBuilder(Func<CompositeAction, CompositeAction> compositeActionBuilder)
        {
            Guard.ForNull(compositeActionBuilder, nameof(compositeActionBuilder));
            return compositeActionBuilder(new EmptyCompositeAction()).Actions;
        }

        /// <summary>
        /// Execute all the actions
        /// </summary>
        /// <param name="actor"></param>
        public Unit ExecuteGivenAs(IActor actor)
        {
            return ExecuteActions(actor);
        }

        /// <summary>
        /// Execute all the actions
        /// </summary>
        /// <param name="actor"></param>
        public Unit ExecuteWhenAs(IActor actor)
        {
            return ExecuteActions(actor);
        }

        private Unit ExecuteActions(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            foreach (var action in Actions)
            {
                actor.Execute(action);
            }
            return Unit.Default;
        }

        /// <summary>
        /// Returns a new <see cref="CompositeAction"/> with the given action appended to its action list
        /// </summary>
        /// <param name="action">The action to add</param>
        /// <returns>A new instance of <see cref="CompositeAction"/> containing <paramref name="action"/></returns>
        public CompositeAction And(IAction<Unit> action)
        {
            Guard.ForNull(action, nameof(action));
            return new AnonymousCompositeAction(Actions.Add(action));
        }

        /// <summary>
        /// Returns the action's name
        /// </summary>
        /// <returns>Returns the action's name</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Gets the enumerator for the <see cref="Actions"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IAction<Unit>> GetEnumerator() => (Actions as IEnumerable<IAction<Unit>>).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => (Actions as IEnumerable).GetEnumerator();
    }
}
