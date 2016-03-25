using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Automation.Actions
{
    public class ToDoItem : Tranquire.IAction
    {
        private readonly string _title;

        public ToDoItem(string title)
        {
            _title = title;
        }        

        public T PerformAs<T>(T actor) where T : IActor
        {
            actor.AttemptsTo(Enter.TheValue(_title).Into("new-todo"))
                 .AttemptsTo(Hit.Enter().Into("new-todo"));
            return actor;
        }

        public static IAction RemoveAToDoItem(string item)
        {
            return new RemoveToDoItem(item);
        }

        public static IAction AddAToDoItem(string title)
        {
            return new ToDoItem(title);
        }

        public static IAction AddToDoItems(ImmutableArray<string> items)
        {
            return new CompositePerformable(items.Select(i => new ToDoItem(i)).ToArray());
        }
    }
}
