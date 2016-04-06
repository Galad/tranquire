using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public interface IActionExecutor
    {
        /// <summary>
        /// Execute the given <see cref="IGivenCommand"/> when the actor changes the state of the system in order to use it with <see cref="AttemptsTo(IWhenCommand)"/>
        /// </summary>
        /// <param name="performable">A <see cref="IGivenCommand"/> representing the action performed by the actor</param>
        /// <returns>The current <see cref="IActor"/> instance, allowing to chain calls</returns>
        IActor WasAbleTo(IGivenCommand performable);
        /// <summary>
        /// Execute the given <see cref="IWhenCommand"/> when the actor uses the system
        /// </summary>
        /// <param name="performable">A <see cref="IWhenCommand"/> representing the action performed by the actor</param>
        /// <returns>The current <see cref="IActor"/> instance, allowing to chain calls</returns>
        IActor AttemptsTo(IWhenCommand performable);
    }

    /// <summary>
    /// Represent an actor which use the system under test. The actor can be given capabilities, such as browsing the web, with the method <see cref="Can{T}(T)"/>
    /// </summary>
    public interface IActor
    {
        IActor Execute(IAction action);
        /// <summary>
        /// Give to the actor the given capability
        /// </summary>
        /// <typeparam name="T">The type of the capability</typeparam>
        /// <param name="doSomething">The capability</param>
        /// <returns>The current <see cref="IActor"/> instance, allowing to chain calls</returns>
        IActor Can<T>(T doSomething) where T : class, IAbility<T>;
        /// <summary>
        /// Retrieve an actor's ability
        /// </summary>
        /// <typeparam name="TAbility">The type of ability to retrieve</typeparam>
        /// <returns>The ability</returns>
        /// <exception cref="InvalidOperationException">The actor does not have the requested ability</exception>
        TAbility AbilityTo<TAbility>() where TAbility : class, IAbility<TAbility>;
        /// <summary>
        /// Ask a question about the current state of the system
        /// </summary>
        /// <typeparam name="TAnswer">Type answer's type</typeparam>
        /// <param name="question">A <see cref="IQuestion{TAnswer}"/> instance representing the question to ask</param>
        /// <returns>The answer to the question.</returns>
        TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question);
    }
}
