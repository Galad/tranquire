using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions.Clicks;

namespace Tranquire.Selenium.Actions
{
    public class Click
    {
        public static ClickOnAction On(ITarget target)
        {
            return new ClickOnAction(target);
        }        
    }
}
