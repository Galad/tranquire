using System.Threading.Tasks;
using Tranquire;

namespace ToDoList.Automation
{
    public static class Remove
    {
        public static IAction<Task> TheToDoItem(string title) => Actions.CreateTagged(
            $"Remove the item {title}",
            (TestLevel.Api, Api.Remove.ToDoItem(title)),
            (TestLevel.UI, UI.Remove.TheToDoItem(title).Select(_ => Task.CompletedTask))
            );
    }
}
