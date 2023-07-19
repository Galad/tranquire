using Tranquire;

namespace ToDoList.Automation.UI;

public static class Add
{
    public static IAction<Unit> TheToDoItem(string title) => ToDoPage.AddToDoItem(title);
}
