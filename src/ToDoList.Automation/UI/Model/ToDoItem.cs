using System;
using Tranquire.Selenium.Questions.UIModels;

namespace ToDoList.Automation.Model
{
    /// <summary>
    /// Represents a To do item in the user interface
    /// </summary>
    public sealed class UIToDoItem
    {
        [Target(ByMethod.CssSelector, "div label")]
        public string Name { get; }

        [Target]
        [Classes(Contains = "completed")]
        public bool Completed { get; }

        /// <summary>Record Constructor</summary>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="completed"><see cref="Completed"/></param>
        public UIToDoItem(string name, bool completed)
        {
            Name = name;
            Completed = completed;
        }

        public ToDoItem ToToDoItem() => new ToDoItem(Guid.Empty, Name, Completed);
    }
}
