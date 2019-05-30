using System.Collections.Immutable;
using System.Linq;
using ToDoList.Automation.Model;
using Tranquire;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.UI
{
    public static class Get
    {
        public static IQuestion<ImmutableArray<ToDoItem>> ToDoItems => ToDoPage.ToDoItems;
        public static IQuestion<int> RemainingItems => ToDoPage.RemainingItems;
    }
}
