﻿using System.Collections.Immutable;
using ToDoList.Automation.Actions;
using ToDoList.Automation.Questions.Converters;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.Questions
{
    public class DisplayedItems : QuestionBase<WebBrowser, ImmutableArray<Model.ToDoItem>>
    {
        protected override ImmutableArray<Model.ToDoItem> Answer(IActor actor, WebBrowser ability)
        {
            return actor.AsksFor(Element.Of(ToDoPage.ToDoItem).Many().As(new WebElementToToDoItemConverter(actor, ability.Driver)));
        }

        public override string Name => "Displayed items";
    }
}