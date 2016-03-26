using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Targets;

namespace Tranquire.Selenium
{
    public static class Target
    {
        public static TargetBuilder The(string friendlyName)
        {
            return new TargetBuilder(friendlyName);
        }
    }
}
