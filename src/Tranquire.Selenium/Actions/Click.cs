using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions.Clicks;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Creates click actions
    /// </summary>
    public class Click
    {
        /// <summary>
        /// Returns a click action on the given target
        /// </summary>
        /// <param name="target">The target where to click</param>
        /// <returns></returns>
        public static ClickOnAction On(ITarget target)
        {
            return new ClickOnAction(target);
        }        
    }
}
