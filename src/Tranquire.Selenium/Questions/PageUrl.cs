using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// A question returning the page URL
    /// </summary>
    public class PageUrl : IQuestion<string, BrowseTheWeb>
    {
        /// <summary>
        ///  Returns the page URL
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public string AnsweredBy(IActor actor, BrowseTheWeb ability)
        {
            return ability.Driver.Url;
        }
    }
}
