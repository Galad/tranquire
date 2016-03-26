using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium;

namespace Tranquire.Selenium.Actions
{
    public interface ITargetableAction<TAction> where TAction : IAction
    {
        TAction Into(ITarget target);
    }
}
