using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an action on the system
    /// </summary>
    public abstract class Action : IAction
    {
        public T ExecuteGivenAs<T>(T actor) where T : class, IActor
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteGiven(new WasAbleToActionCommand<T>(actor), actor);
            return actor;
        }

        public T ExecuteWhenAs<T>(T actor) where T : class, IActor
        {
            Guard.ForNull(actor, nameof(actor));
            ExecuteWhen(new AttemptsToActionCommand<T>(actor), actor);
            return actor;
        }

        protected abstract void ExecuteWhen(IActionCommand command, IActor actor);

        protected virtual void ExecuteGiven(IActionCommand command, IActor actor)
        {
            ExecuteWhen(command, actor);
        }

        public interface IActionCommand
        {
            void Execute(IAction action);
        }

        private class AttemptsToActionCommand<T> : IActionCommand where T : class, IActor
        {
            private readonly T _actor;

            public AttemptsToActionCommand(T actor)
            {
                _actor = actor;
            }

            public void Execute(IAction action)
            {
                _actor.AttemptsTo(action);
            }
        }

        private class WasAbleToActionCommand<T> : IActionCommand where T : class, IActor
        {
            private readonly T _actor;

            public WasAbleToActionCommand(T actor)
            {
                _actor = actor;
            }

            public void Execute(IAction action)
            {
                _actor.WasAbleTo(action);
            }
        }
    }
}
