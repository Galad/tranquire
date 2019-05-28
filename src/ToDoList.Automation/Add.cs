using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquire;

namespace ToDoList.Automation
{
    public static class Add
    {
        public static IAction<Task> TheToDoItem(string title) => Actions.CreateTagged(
            $"Add the to-do item {title}",
            (TestLevel.UI, UI.Add.TheToDoItem(title).Select(_ => Task.CompletedTask)),
            (TestLevel.Api, Api.Add.TheToDoItem(title))
            );

        public static IAction<Task> TheToDoItems(IEnumerable<string> titles) => Actions.Create(
            "Add to-do items",
            async actor =>
            {
                foreach (var title in titles)
                {
                    await actor.Execute(TheToDoItem(title));
                }
            });
    }
}
