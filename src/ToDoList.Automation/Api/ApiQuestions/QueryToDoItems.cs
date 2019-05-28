using Newtonsoft.Json;
using System;
using System.Collections.Immutable;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoList.Automation.Model;
using Tranquire;

namespace ToDoList.Automation.Api.ApiQuestions
{
    /// <summary>
    /// Gets the list of to-do items via the API
    /// </summary>
    public class QueryToDoItems : QuestionBase<HttpClient, Task<ImmutableArray<ToDoItem>>>
    {
        public override string Name => "Gets the to-do items via the API";

        protected override async Task<ImmutableArray<ToDoItem>> Answer(IActor actor, HttpClient ability)
        {
            var result = await ability.GetAsync("api/todoitem").ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var contentString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<ImmutableArray<ToDoItem>>(contentString);
        }
    }
}
