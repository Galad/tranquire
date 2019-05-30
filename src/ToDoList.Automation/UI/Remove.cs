using Tranquire;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.UI
{
    public static class Remove
    {
        public static IAction<Unit> TheToDoItem(string title) => ToDoPage.RemoveToDoItem(title);
    }
}
