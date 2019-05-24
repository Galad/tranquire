using Tranquire.Selenium.Questions.UIModels;

namespace ToDoList.Automation.Model
{
    public sealed class ToDoItem
    {
        [Target(ByMethod.CssSelector, "div label")]
        public string Name { get; }

        [Target]
        [Classes(Contains = "completed")]
        public bool IsCompleted { get; }

        /// <summary>Record Constructor</summary>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="isCompleted"><see cref="IsCompleted"/></param>
        public ToDoItem(string name, bool isCompleted)
        {
            Name = name;
            IsCompleted = isCompleted;
        }
    }
}
