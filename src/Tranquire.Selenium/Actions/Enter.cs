using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions.Enters;

namespace Tranquire.Selenium.Actions
{
    public class Enter : TargetableAction<EnterValue>
    {
        private readonly string value;

        public Enter(string value) : base(t => new EnterValue(value, t))
        {
            this.value = value;
        }

        public static Enter TheValue(string value)
        {
            return new Enter(value);
        }
    }
}
