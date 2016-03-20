using OpenQA.Selenium;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Tranquire.Selenium
{
    public class StringArrayTextValue : IQuestion<ImmutableArray<string>>
    {        
        private string id;

        public StringArrayTextValue(string id)
        {
            this.id = id;
        }

        public ImmutableArray<string> AnsweredBy(IActor actor)
        {
           return actor.AbilityTo<BrowseTheWeb>()
                       .Driver
                       .FindElements(By.CssSelector(id))
                       .Select(e => e.Text)
                       .ToImmutableArray();
        }
    }
}