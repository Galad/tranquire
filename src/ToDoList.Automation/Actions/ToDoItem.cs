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
    public class ToDoItem : Tranquire.ITask
    {
        private readonly string _title;

        public ToDoItem(string title)
        {
            _title = title;
        }        

        public T PerformAs<T>(T actor) where T : IActor
        {
            actor.AttemptsTo(Enter.TheValue(_title).Into(ToDoPage.NewToDoItemInput))
                 .AttemptsTo(Hit.Enter().Into(ToDoPage.NewToDoItemInput));
            return actor;
        }

        public static ITask RemoveAToDoItem(string item)
        {
            return new RemoveToDoItem(item);
        }

        public static ITask AddAToDoItem(string title)
        {
            return new ToDoItem(title);
        }

        public static ITask AddToDoItems(ImmutableArray<string> items)
        {
            return new Tranquire.Task(items.Select(i => new ToDoItem(i)).ToArray());
        }
    }
}
