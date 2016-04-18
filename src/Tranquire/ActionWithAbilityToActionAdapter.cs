using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Provides an adapter for <see cref="IAction{TGiven, TWhen}"/> types so that an action with an ability can be used as an action without ability
    /// </summary>
    /// <typeparam name="TGiven">Type of the Given ability</typeparam>
    /// <typeparam name="TWhen">Type of the When ability</typeparam>
    public class ActionWithAbilityToActionAdapter<TGiven, TWhen> : IAction
    {
        /// <summary>
        /// Return the adapted action
        /// </summary>
        public IAction<TGiven, TWhen> Action { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ActionWithAbilityToActionAdapter{TGiven, TWhen}"/>
        /// </summary>
        /// <param name="action">The action to adapt</param>
        public ActionWithAbilityToActionAdapter(IAction<TGiven, TWhen> action)
        {
            Guard.ForNull(action, nameof(action));
            Action = action;
        }

        /// <summary>
        /// Execute the action with the ability
        /// </summary>
        /// <param name="actor"></param>
        public void ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            actor.Execute(Action);
        }

        /// <summary>
        /// Execute the action with the ability
        /// </summary>
        /// <param name="actor"></param>
        public void ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            actor.Execute(Action);
        }
    }
}
