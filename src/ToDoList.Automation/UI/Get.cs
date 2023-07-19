using System.Collections.Immutable;
using ToDoList.Automation.Model;
using Tranquire;

namespace ToDoList.Automation.UI;

public static class Get
{
    public static IQuestion<ImmutableArray<ToDoItem>> ToDoItems => ToDoPage.ToDoItems;
    public static IQuestion<int> RemainingItems => ToDoPage.RemainingItems;
}
