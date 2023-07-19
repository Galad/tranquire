using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tranquire;

namespace ToDoList.Automation.Api.ApiActions;

public class RemoveToDoItem : ActionBase<HttpClient, Task>
{
    public RemoveToDoItem(Guid id)
    {
        Id = id;
    }

    public override string Name => $"Remove the to-do item {Id}";

    public Guid Id { get; }

    protected async override Task ExecuteWhen(IActor actor, HttpClient ability)
    {
        var result = await ability.DeleteAsync("api/todoitem/" + Uri.EscapeUriString(Id.ToString())).ConfigureAwait(false);
        result.EnsureSuccessStatusCode();
    }
}
