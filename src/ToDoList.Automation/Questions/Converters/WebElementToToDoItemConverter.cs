using OpenQA.Selenium;
using System;
using System.Globalization;
using ToDoList.Automation.Actions;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Questions;
using Tranquire.Selenium.Questions.Converters;

namespace ToDoList.Automation.Questions.Converters
{
    public sealed class WebElementToToDoItemConverter : IConverter<IWebElement, Model.ToDoItem>
    {
        private readonly IActor _actor;
        private readonly IWebDriver _webDriver;

        public WebElementToToDoItemConverter(IActor actor, IWebDriver webDriver)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (webDriver == null)
            {
                throw new ArgumentNullException(nameof(webDriver));
            }
            _webDriver = webDriver;
            _actor = actor;
        }

        public Model.ToDoItem Convert(IWebElement source, CultureInfo culture)
        {
            var target = Target.The("To do item").LocatedByWebElement(source);
            return new Model.ToDoItem(
                _actor.AsksFor(TextContent.Of(ToDoPage.ToDoItemName.RelativeTo(target)).Value),
                target.ResolveFor(_webDriver).GetAttribute("class").Contains("completed")
                );
        }
    }
}
