using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Questions
{
    public class PageTitle : IQuestion<string>
    {
        public string AnsweredBy(IActor actor)
        {
            return actor.BrowseTheWeb().Title;
        }
    }

    public class Page
    {
        public static IQuestion<string> Title()
        {
            return new PageTitle();
        }
    }
}
