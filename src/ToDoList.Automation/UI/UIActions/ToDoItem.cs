using Tranquire;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.UI.UIActions
{
    public class AddToDoItem : CompositeAction
    {
        public string Title { get; }

        public AddToDoItem(string title) :
            base(t => t.And(Enter.TheValue(title).Into(ToDoPage.NewToDoItemInput))
                       .And(Hit.Enter().Into(ToDoPage.NewToDoItemInput)))
        {
            Title = title;
        }

        public override string Name => "Add item '" + Title + "'";
    }
}
