using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    public class Text
    {
        private string id;

        public Text(string id)
        {
            this.id = id;
        }

        public static Text Of(string id)
        {
            return new Text(id);
        }

        public StringArrayTextValue AsStringArray()
        {
            return new StringArrayTextValue(id);
        }

        public IntegerTextValue AsInteger()
        {
            return new IntegerTextValue(id);
        }
    }
}
