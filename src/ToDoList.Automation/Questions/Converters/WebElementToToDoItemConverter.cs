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
using System.Globalization;

namespace ToDoList.Automation.Questions.Converters
{
    public sealed class WebElementToToDoItemConverter : IConverter<IWebElement, Model.ToDoItem>
    {
        private readonly IActor _actor;
        private readonly IWebDriver _webDriver;

        public WebElementToToDoItemConverter(IActor actor, IWebDriver webDriver)
        {
            if(actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if(webDriver == null)
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
                _actor.AsksFor(Text.Of(ToDoPage.ToDoItemName.RelativeTo(target)).Value),
                target.ResolveFor(_webDriver).GetAttribute("class").Contains("completed")
                );
        }
    }
}
