using System.Threading.Tasks;
using ToDoList.Automation.Api.ApiActions;
using Tranquire;

namespace ToDoList.Automation.Api
{
    public static class Remove
    {
        public static IAction<Task> ToDoItem(string title) => Get.TheToDoItem(title)
                                                                 .SelectMany<Task<Model.ToDoItem>, Task>(itemTask =>
                                                                 {
                                                                     return Actions.Create(
                                                                         $"Remove to-do item",
                                                                         async actor =>
                                                                         {
                                                                             var item = await itemTask;
                                                                             return actor.Execute(new RemoveToDoItem(item.Id));
                                                                         });
                                                                 });
    }
}
