using Tranquire;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.UI
{
    public static class Remove
    {
        public static IAction<Unit> TheToDoItem(string title) => 
            new DefaultCompositeAction($"Remove the to-do item {title}",
                Open.TheApplication(),
                JsClick.On(ToDoPage.RemoveToDoItemButton.Of(title)));
    }
}
