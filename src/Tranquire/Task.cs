using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent a <see cref="IAction{TResult}"/> composed of several <see cref="IAction{TResult}"/>
    /// </summary>    
    public abstract class Task : IAction<Unit>
    {
        private class EmptyTask : Task
        {
            public override string Name => "Empty task";
        }

        private class CompositeTask : Task
        {
            public override string Name => "Multiple tasks";
            public CompositeTask(IEnumerable<IAction<Unit>> actions) : base(actions)
            {
            }
        }

        /// <summary>
        /// Gets the actions executed by this task
        /// </summary>
        public IEnumerable<IAction<Unit>> Actions { get; }
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Creates a new instance of task
        /// </summary>
        /// <param name="actions">The actions executed by this task</param>
        protected Task(IEnumerable<IAction<Unit>> actions)
        {
            Guard.ForNull(actions, nameof(actions));
            Actions = actions;
        }

        /// <summary>
        /// Creates a new instance of task
        /// </summary>
        /// <param name="actions">The actions executed by this task</param>
        protected Task(params IAction<Unit>[] actions) : this(actions as IEnumerable<IAction<Unit>>)
        {
        }

        /// <summary>
        /// Creates a new instance of task using a builder
        /// </summary>
        /// <param name="taskBuilder">A function taking an empty task and returning a task containing the actions to execute</param>
        protected Task(Func<Task, Task> taskBuilder) : this(GetActionsFromTaskBuilder(taskBuilder))
        {
        }

        private static IEnumerable<IAction<Unit>> GetActionsFromTaskBuilder(Func<Task, Task> taskBuilder)
        {
            Guard.ForNull(taskBuilder, nameof(taskBuilder));
            return taskBuilder(new EmptyTask()).Actions;
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
            foreach (var task in Actions)
            {
                actor.Execute(task);
            }
            return Unit.Default;
        }

        /// <summary>
        /// Returns a new <see cref="Task"/> with the given action appended to its action list
        /// </summary>
        /// <param name="action">The action to add</param>
        /// <returns>A new instance of <see cref="Task"/> containing <paramref name="action"/></returns>
        public Task And(IAction<Unit> action)
        {
            Guard.ForNull(action, nameof(action));
            return new CompositeTask(Actions.Concat(new[] { action }));
        }

        /// <summary>
        /// Returns a new <see cref="Task"/> with the given action using an ability appended to its action list
        /// </summary>
        /// <param name="action">The action to add</param>
        /// <returns>A new instance of <see cref="Task"/> containing <paramref name="action"/></returns>
        public Task And<TGiven, TWhen>(IAction<TGiven, TWhen, Unit> action)
        {
            Guard.ForNull(action, nameof(action));
            return new CompositeTask(Actions.Concat(new[] { new ActionWithAbilityToActionAdapter<TGiven, TWhen, Unit>(action) }));
        }

        /// <summary>
        /// Returns the task's name
        /// </summary>
        /// <returns>Returns the task's name</returns>
        public override string ToString() => Name;
    }
}
