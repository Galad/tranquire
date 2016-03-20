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
            Name = name;
        }

        public T AbilityTo<T>() where T : IAbility<T>
        {
            return (T)_abilities[typeof(T)];
        }

        public IActor AttemptsTo(IAction performable)
        {
            return performable.PerformAs(this);
        }

        public IActor Can<T>(T doSomething) where T : IAbility<T>
        {
            _abilities[typeof(T)] = doSomething;
            return this;
        }

        public IActor WasAbleTo(ITask performable)
        {
            return performable.PerformAs(this);
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return question.AnsweredBy(this);
        }
    }
}
