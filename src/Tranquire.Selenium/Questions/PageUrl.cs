using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Questions
{
    public class PageUrl : IQuestion<string>
    {
        public string AnsweredBy(IActor actor)
        {
            return actor.BrowseTheWeb().Url;
        }
    }
}
