using System;
using System.Collections.Immutable;
using ToDoList.Automation.Actions;
using ToDoList.Automation.Questions.Converters;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.Questions
{
    public class DisplayedItems : IQuestion<ImmutableArray<Model.ToDoItem>, WebBrowser>
    {
        public ImmutableArray<Model.ToDoItem> AnsweredBy(IActor actor, WebBrowser ability)
        {
            return actor.AsksFor(Element.Of(ToDoPage.ToDoItem).Many().As(new WebElementToToDoItemConverter(actor, ability.Driver)));
        }

        public string Name => "Displayed items";
    }
}