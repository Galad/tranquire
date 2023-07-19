using System.Collections.Immutable;
using System.Linq;
using OpenQA.Selenium;
using ToDoList.Automation.Model;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.UI;

public static class ToDoPage
{
    private static readonly ITargetWithParameters RemoveToDoItemButton =
        Target.The("Remove an item button")
              .LocatedBy("//*[contains(@class, 'todo-list')]/li[div[contains(.,'{0}')]]//button[@class='destroy']", By.XPath);

    private static readonly ITarget NewToDoItemInput = Target.The("New todo item input").LocatedBy(By.Id("new-todo"));
    private static readonly ITarget ToDoItemsLeftCounter = Target.The("To do items left counter").LocatedBy(By.Id("todo-count"));
    private static readonly ITarget ToDoItem = Target.The("To do item").LocatedBy(By.CssSelector("ul.todo-list li"));

    public static IAction<Unit> AddToDoItem(string title)
    {
        return new DefaultCompositeAction(
            $"Add item '{title}'",
            Enter.TheValue(title).Into(NewToDoItemInput),
            Hit.Enter().Into(NewToDoItemInput));
    }

    public static IAction<Unit> RemoveToDoItem(string title)
    {
        return new DefaultCompositeAction($"Remove the to-do item {title}",
            Open.TheApplication(),
            JsClick.On(ToDoPage.RemoveToDoItemButton.Of(title)));
    }

    public static IQuestion<ImmutableArray<ToDoItem>> ToDoItems { get; } = UIModel.Of<UIToDoItem>(ToDoItem)
                                                                                  .Many()
                                                                                  .Select(items => items.Select(i => i.ToToDoItem()).ToImmutableArray());

    public static IQuestion<int> RemainingItems { get; } = TextContent.Of(ToDoItemsLeftCounter).AsInteger();
}
