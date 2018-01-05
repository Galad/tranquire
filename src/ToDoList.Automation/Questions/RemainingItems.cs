using ToDoList.Automation.Actions;
using Tranquire;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.Questions
{
    public class RemainingItems : IQuestion<int>
    {
        public int AnsweredBy(IActor actor)
        {
            return actor.AsksFor(TextContent.Of(ToDoPage.ToDoItemsLeftCounter).AsInteger());
        }

        public string Name => "Remaining items";
    }
}