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
    public class PageUrl : IQuestion<string, WebBrowser>
    {
        /// <summary>
        ///  Returns the page URL
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        public string AnsweredBy(IActor actor, WebBrowser ability)
        {
            return ability.Driver.Url;
        }

        /// <summary>
        /// Gets the action's name
        /// </summary>
        public string Name => "Page URL";
    }
}
