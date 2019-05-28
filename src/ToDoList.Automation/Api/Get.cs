using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Automation.Api.ApiQuestions;
using ToDoList.Automation.Model;
using Tranquire;

namespace ToDoList.Automation.Api
{
    public static class Get
    {
        public static IQuestion<Task<ImmutableArray<ToDoItem>>> TheToDoItems { get; } = new QueryToDoItems();
        public static IQuestion<Task<ToDoItem>> TheToDoItem(string title) =>
            TheToDoItems.Select(async itemsTask =>
            {
                var items = await itemsTask;
                return items.FirstOrDefault(i => i.Name == title);
            });
    }
}
