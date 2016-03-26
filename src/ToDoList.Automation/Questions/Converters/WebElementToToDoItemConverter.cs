using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions.Converters;
using ToDoList.Automation.Model;
using Tranquire.Selenium;
using Tranquire;
using Tranquire.Selenium.Questions;
using ToDoList.Automation.Actions;

namespace ToDoList.Automation.Questions.Converters
{
    public sealed class WebElementToToDoItemConverter : ExplicitConverter<IWebElement, Model.ToDoItem>
    {
        private readonly IActor _actor;

        public WebElementToToDoItemConverter(IActor actor)
        {
            _actor = actor;
        }

        public override Model.ToDoItem Convert(IWebElement source)
        {
            var target = Target.The("To do item").LocatedByWebElement(source);
            return new Model.ToDoItem(
                _actor.AsksFor(Text.Of(ToDoPage.ToDoItemName.RelativeTo(target)).Value),
                target.ResolveFor(_actor).GetAttribute("class").Contains("completed")
                );
        }
    }
}
