using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent a <see cref="IAction"/> composed of several <see cref="IAction"/>
    /// </summary>
    public class Task : IAction
    {
        /// <summary>
        /// Gets the actions executed by this task
        /// </summary>
        public IEnumerable<IAction> Actions { get; }

        /// <summary>
        /// Creates a new instance of task
        /// </summary>
        /// <param name="actions">The actions executed by this task</param>
        public Task(IEnumerable<IAction> actions)
        {
            Guard.ForNull(actions, nameof(actions));
            Actions = actions;
        }

        /// <summary>
        /// Creates a new instance of task
        /// </summary>
        /// <param name="actions">The actions executed by this task</param>
        public Task(params IAction[] actions) : this(actions as IEnumerable<IAction>)
        {
        }

        /// <summary>
        /// Creates a new instance of task using a builder
        /// </summary>
        /// <param name="taskBuilder">A function taking an empty task and returning a task containing the actions to execute</param>
        public Task(Func<Task, Task> taskBuilder) : this(GetActions(taskBuilder))
        {
        }

        private static IEnumerable<IAction> GetActions(Func<Task, Task> taskBuilder)
        {
            Guard.ForNull(taskBuilder, nameof(taskBuilder));
            return taskBuilder(new Task()).Actions;
        }

        /// <summary>
        /// Execute all the actions
        /// </summary>
        /// <param name="actor"></param>
        public void ExecuteGivenAs(IActor actor)
        {
            ExecuteActions(actor);
        }
       
        /// <summary>
        /// Execute all the actions
        /// </summary>
        /// <param name="actor"></param>
        public void ExecuteWhenAs(IActor actor)
        {

            ExecuteActions(actor);
        }

        private void ExecuteActions(IActor actor)
        {
            Guard.ForNull(actor, nameof(actor));
            foreach (var task in Actions)
            {
                actor.Execute(task);
            }
        }

        /// <summary>
        /// Returns a new <see cref="Task"/> with the given action appended to its action list
        /// </summary>
        /// <param name="action">The action to add</param>
        /// <returns>A new instance of <see cref="Task"/> containing <paramref name="action"/></returns>
        public Task And(IAction action)
        {
            Guard.ForNull(action, nameof(action));
            return new Task(Actions.Concat(new[] { action }));
        }

        /// <summary>
        /// Returns a new <see cref="Task"/> with the given action using an ability appended to its action list
        /// </summary>
        /// <param name="action">The action to add</param>
        /// <returns>A new instance of <see cref="Task"/> containing <paramref name="action"/></returns>
        public Task And<TGiven, TWhen>(IAction<TGiven, TWhen> action)
        {
            Guard.ForNull(action, nameof(action));
            return new Task(Actions.Concat(new[] { new ActionWithAbilityToActionAdapter<TGiven, TWhen>(action) }));
        }
    }
}
