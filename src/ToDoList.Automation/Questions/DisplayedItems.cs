using System.Collections.Immutable;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.Questions
{
    public class DisplayedItems : QuestionBase<WebBrowser, ImmutableArray<Model.ToDoItem>>
    {
        protected override ImmutableArray<Model.ToDoItem> Answer(IActor actor, WebBrowser ability)
        {
            return actor.AsksFor(UIModel.Of<Model.ToDoItem>(Actions.ToDoPage.ToDoItem).Many());
        }

        public override string Name => "Displayed items";
    }
}