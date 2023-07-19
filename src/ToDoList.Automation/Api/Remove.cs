using System.Threading.Tasks;
using ToDoList.Automation.Api.ApiActions;
using Tranquire;

namespace ToDoList.Automation.Api;

public static class Remove
{
    public static IAction<Task> ToDoItem(string title) => Get.TheToDoItem(title)
                                                             .SelectMany(item => new RemoveToDoItem(item.Id));
}
