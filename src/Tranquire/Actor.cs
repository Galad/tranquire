using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public class Actor : IActor
    {
        private readonly Dictionary<Type, IAbility> _abilities = new Dictionary<Type, IAbility>();

        public string Name { get; }

        public Actor(string name)
        {
            Guard.ForNull(name, nameof(name));
            Name = name;
        }
                
        public T AbilityTo<T>() where T : IAbility<T>
        {
            IAbility ability;
            if(!_abilities.TryGetValue(typeof(T), out ability))
            {
                throw new InvalidOperationException($"The ability {typeof(T).Name} was requested but the actor {Name} does not have it.");
            }
            return (T)_abilities[typeof(T)];
        }

        public IActor AttemptsTo(IPerformable performable)
        {
            Guard.ForNull(performable, nameof(performable));
            return performable.PerformAs(this);
        }

        public IActor WasAbleTo(IPerformable performable)
        {
            Guard.ForNull(performable, nameof(performable));
            return performable.PerformAs(this);
        }

        public IActor Can<T>(T doSomething) where T : IAbility<T>
        {
            var ability = (IAbility)doSomething;
            Guard.ForNull(ability, nameof(doSomething));
            _abilities[typeof(T)] = doSomething;            
            return this;
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            Guard.ForNull(question, nameof(question));
            return question.AnsweredBy(this);
        }
    }
}
