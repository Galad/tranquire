using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Represent an action which is performed on a target
    /// </summary>
    /// <typeparam name="TAction"></typeparam>
    public interface ITargetableAction<TAction>
    {
        /// <summary>
        /// Specifies on what target the action should be performed
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        TAction Into(ITarget target);
    }
}
