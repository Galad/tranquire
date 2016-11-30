using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.Actions
{
    public class ToDoItem : Tranquire.Task
    {
        public string Title { get; }

        public ToDoItem(string title) :
            base(t => t.And(Enter.TheValue(title).Into(ToDoPage.NewToDoItemInput))
                       .And(Hit.Enter().Into(ToDoPage.NewToDoItemInput)))
        {
            Title = title;
        }

        public static IAction<Unit> RemoveAToDoItem(string item)
        {
            return new RemoveToDoItem(item);
        }

        public static IAction<Unit> AddAToDoItem(string title)
        {
            return new ToDoItem(title);
        }

        public static IAction<Unit> AddToDoItems(ImmutableArray<string> items)
        {
            return new AddToDoItemsAction(items.Select(i => new ToDoItem(i)).ToArray());
        }

        private class AddToDoItemsAction : Tranquire.Task
        {
            public AddToDoItemsAction(ToDoItem[] actions) : base(actions)
            {
            }

            public override string Name => "Add to do items";
        }

        public override string Name => "Add item '" + Title + "'";
    }
}
