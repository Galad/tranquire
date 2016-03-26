using System;
using System.Collections.Immutable;
using ToDoList.Automation.Actions;
using ToDoList.Automation.Questions.Converters;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.Questions
{
    public class DisplayedItems : IQuestion<ImmutableArray<Model.ToDoItem>>
    {
        public ImmutableArray<Model.ToDoItem> AnsweredBy(IActor actor)
        {
            return actor.AsksFor(Element.Of(ToDoPage.ToDoItem).WithCustomConverter(new WebElementToToDoItemConverter(actor)).Many().As<Model.ToDoItem>());
        }
    }
}