using System.Collections.Immutable;
using System.Linq;
using ToDoList.Automation.Model;
using Tranquire;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.UI
{
    public static class Get
    {
        public static IQuestion<ImmutableArray<ToDoItem>> ToDoItems { get; } = UIModel.Of<UIToDoItem>(ToDoPage.ToDoItem)
                                                                                      .Many()
                                                                                      .Select(items => items.Select(i => i.ToToDoItem()).ToImmutableArray());
        public static IQuestion<int> RemainingItems { get; } = TextContent.Of(ToDoPage.ToDoItemsLeftCounter).AsInteger();
    }
}
