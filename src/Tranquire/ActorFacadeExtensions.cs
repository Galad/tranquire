using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Tranquire
{
    /// <summary>
    /// Contains extension methods for <see cref="IActorFacade"/>
    /// </summary>
    public static class ActorFacadeExtensions
    {
        /// <summary>
        /// Execute all the given <see cref = "IGivenCommand{TResult}"/> when the actor uses the system
        /// </summary>
        /// <param name = "commands">A list of <see cref = "IGivenCommand{TResult}"/> representing the actions performed by the actor</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <param name="actor">The actor used to perform the actions</param>
        /// <returns>An array of all the command results</returns>
        public static ImmutableArray<TResult> Given<TResult>(this IActorFacade actor, params IGivenCommand<TResult>[] commands)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            return commands.Select(c => actor.Given(c)).ToImmutableArray();
        }

        /// <summary>
        /// Execute all the given <see cref = "IWhenCommand{TResult}"/> when the actor uses the system
        /// </summary>
        /// <param name = "commands">A list of <see cref = "IWhenCommand{TResult}"/> representing the actions performed by the actor</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <param name="actor">The actor used to perform the actions</param>
        /// <returns>An array of all the command results</returns>
        public static ImmutableArray<TResult> When<TResult>(this IActorFacade actor, params IWhenCommand<TResult>[] commands)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            return commands.Select(c => actor.When(c)).ToImmutableArray();
        }

        /// <summary>
        /// Give an ability to the actor if the predicate is true
        /// </summary>
        /// <typeparam name="T">The type of the ability</typeparam>
        /// <param name="actor"></param>
        /// <param name="doSomething">Ability</param>
        /// <param name="predicate">A value indicating if the ability should be added</param>
        /// <returns>A new actor with the given ability</returns>
        public static IActorFacade CanUseIf<T>(this IActorFacade actor, Func<T> doSomething, bool predicate)
            where T : class
        {
            if (actor is null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (doSomething is null)
            {
                throw new ArgumentNullException(nameof(doSomething));
            }
            if (predicate)
            {
                return actor.CanUse(doSomething());
            }

            return actor;
        }
    }
}
