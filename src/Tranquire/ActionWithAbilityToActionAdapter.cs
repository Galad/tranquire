using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Provides an adapter for <see cref="IAction{TGiven, TWhen, TResult}"/> types so that an action with an ability can be used as an action without ability
    /// </summary>
    /// <typeparam name="TGiven">Type of the Given ability</typeparam>
    /// <typeparam name="TWhen">Type of the When ability</typeparam>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public class ActionWithAbilityToActionAdapter<TGiven, TWhen, TResult> : IAction<TResult>
    {
        /// <summary>
        /// Return the adapted action
        /// </summary>
        public IAction<TGiven, TWhen, TResult> Action { get; }
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public string Name => Action.Name;

        /// <summary>
        /// Creates a new instance of <see cref="ActionWithAbilityToActionAdapter{TGiven, TWhen, TResult}"/>
        /// </summary>
        /// <param name="action">The action to adapt</param>
        public ActionWithAbilityToActionAdapter(IAction<TGiven, TWhen, TResult> action)
        {
            Guard.ForNull(action, nameof(action));
            Action = action;
        }

        /// <summary>
        /// Execute the action with the ability
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteGivenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return actor.ExecuteWithAbility(Action);
        }

        /// <summary>
        /// Execute the action with the ability
        /// </summary>
        /// <param name="actor"></param>
        public TResult ExecuteWhenAs(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            return actor.ExecuteWithAbility(Action);
        }

        /// <summary>
        /// Returns the action's name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Action.ToString();
    }
}
