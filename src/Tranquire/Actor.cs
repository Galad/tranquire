using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an actor which use the system under test. The actor can be given capabilities, such as browsing the web, with the method <see cref="Can{T}(T)"/>
    /// </summary>
    public class Actor : IActor
    {
        private readonly Dictionary<Type, IAbility> _abilities = new Dictionary<Type, IAbility>();

        /// <summary>
        /// Gets the actor name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Create a new instance of <see cref="Actor"/>
        /// </summary>
        /// <param name="name">The actor's name</param>
        public Actor(string name)
        {
            Guard.ForNull(name, nameof(name));
            Name = name;
        }
                
        /// <summary>
        /// Retrieve an actor's ability
        /// </summary>
        /// <typeparam name="T">The type of ability to retrieve</typeparam>
        /// <returns>The ability</returns>
        /// <exception cref="InvalidOperationException">The actor does not have the requested ability</exception>
        public T AbilityTo<T>() where T : IAbility<T>
        {
            IAbility ability;
            if(!_abilities.TryGetValue(typeof(T), out ability))
            {
                throw new InvalidOperationException($"The ability {typeof(T).Name} was requested but the actor {Name} does not have it.");
            }
            return (T)_abilities[typeof(T)];
        }

        /// <summary>
        /// Execute the given <see cref="IPerformable"/> when the actor uses the system
        /// </summary>
        /// <param name="performable">A <see cref="IPerformable"/> representing the action performed by the actor</param>
        /// <returns>The current <see cref="IActor"/> instance, allowing to chain calls</returns>
        public IActor AttemptsTo(IPerformable performable)
        {
            Guard.ForNull(performable, nameof(performable));
            return performable.PerformAs(this);
        }

        /// <summary>
        /// Execute the given <see cref="IPerformable"/> when the actor changes the state of the system in order to use it with <see cref="AttemptsTo(IPerformable)"/>
        /// </summary>
        /// <param name="performable">A <see cref="IPerformable"/> representing the action performed by the actor</param>
        /// <returns>The current <see cref="IActor"/> instance, allowing to chain calls</returns>
        public IActor WasAbleTo(IPerformable performable)
        {
            Guard.ForNull(performable, nameof(performable));
            return performable.PerformAs(this);
        }

        /// <summary>
        /// Give to the actor the given capability
        /// </summary>
        /// <typeparam name="T">The type of the capability</typeparam>
        /// <param name="doSomething">The capability</param>
        /// <returns>The current <see cref="IActor"/> instance, allowing to chain calls</returns>
        public IActor Can<T>(T doSomething) where T : IAbility<T>
        {
            var ability = (IAbility)doSomething;
            Guard.ForNull(ability, nameof(doSomething));
            _abilities[typeof(T)] = doSomething;            
            return this;
        }

        /// <summary>
        /// Ask a question about the current state of the system
        /// </summary>
        /// <typeparam name="TAnswer">Type answer's type</typeparam>
        /// <param name="question">A <see cref="IQuestion{TAnswer}"/> instance representing the question to ask</param>
        /// <returns>The answer to the question.</returns>
        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            Guard.ForNull(question, nameof(question));
            return question.AnsweredBy(this);
        }
    }
}
