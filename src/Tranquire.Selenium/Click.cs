using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    public class Click
    {
        private string xpath;

        public Click(string xpath)
        {
            this.xpath = xpath;
        }

        public static Click On(string xpath)
        {
            return new Click(xpath);
        }

        public ClickAction ViewedBy(IActor actor)
        {
            return new ClickAction(xpath, actor);
        }
    }
}
