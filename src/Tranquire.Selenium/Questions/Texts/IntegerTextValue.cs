using OpenQA.Selenium;
using System;

namespace Tranquire.Selenium.Questions.Texts
{
    public class IntegerTextValue : IQuestion<int>
    {
        private string _id;

        public IntegerTextValue(string id)
        {
            _id = id;
        }

        public int AnsweredBy(IActor actor)
        {
            return int.Parse(actor.AbilityTo<BrowseTheWeb>()
                       .Driver
                       .FindElement(By.CssSelector(_id))
                       .Text);
        }
    }
}