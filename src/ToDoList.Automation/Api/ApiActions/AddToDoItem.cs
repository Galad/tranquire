using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tranquire;

namespace ToDoList.Automation.Api.ApiActions
{
    public class AddToDoItem : ActionBase<HttpClient, Task>
    {
        public AddToDoItem(string title)
        {
            Title = title;
        }

        public override string Name => $"Add the to-do item {Title}";

        public string Title { get; }

        protected override async Task ExecuteWhen(IActor actor, HttpClient ability)
        {
            var response = await ability.PostAsync("api/todoitem", new StringContent(JsonConvert.SerializeObject(Title), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }
    }
}
