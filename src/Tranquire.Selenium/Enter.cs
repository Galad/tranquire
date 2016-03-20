using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    public class Enter
    {
        private readonly string value;

        public Enter(string value)
        {
            this.value = value;
        }

        public static Enter TheValue(string value)
        {
            return new Enter(value);
        }

        public EnterValue Into(string id)
        {
            return new EnterValue(id, value);
        }
    }
}
