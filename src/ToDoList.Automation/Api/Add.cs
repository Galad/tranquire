using System.Threading.Tasks;
using ToDoList.Automation.Api.ApiActions;
using Tranquire;

namespace ToDoList.Automation.Api;

public static class Add
{
    public static IAction<Task> TheToDoItem(string title) => new AddToDoItem(title);
}
