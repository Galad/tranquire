using System.Collections.Generic;
using System.Linq;
using ToDoList.Automation.UI.UIActions;
using Tranquire;

namespace ToDoList.Automation.UI
{
    public static class Add
    {
        public static IAction<Unit> TheToDoItem(string title) => new AddToDoItem(title);
    }
}
