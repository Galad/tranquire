using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions.Enters;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Creates input keys actions
    /// </summary>
    public class Enter : TargetableAction<EnterValue>
    {
        private readonly string value;

        public Enter(string value) : base(t => new EnterValue(value, t))
        {
            this.value = value;
        }

        /// <summary>
        /// Creates an action which input the given string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Enter TheValue(string value)
        {
            return new Enter(value);
        }
    }
}
