using OpenQA.Selenium;
using Tranquire.Selenium;

namespace ToDoList.Automation.Actions
{
    public static class ToDoPage
    {
        public static readonly ITargetWithParameters RemoveToDoItemButton =
            Target.The("Remove an item button")
                  .LocatedBy("//*[contains(@class, 'todo-list')]/li[div[contains(.,'{0}')]]//button[@class='destroy']", By.XPath);
        public static readonly ITarget NewToDoItemInput = Target.The("New todo item input").LocatedBy(By.Id("new-todo"));
        public static readonly ITarget ToDoItemsLeftCounter = Target.The("To do items left counter").LocatedBy(By.Id("todo-count"));
        public static readonly ITarget ToDoItem = Target.The("To do item").LocatedBy(By.CssSelector("ul.todo-list li"));
        public static readonly ITarget ToDoItemName = Target.The("To do item name").LocatedBy(By.CssSelector("div label"));
    }
}
