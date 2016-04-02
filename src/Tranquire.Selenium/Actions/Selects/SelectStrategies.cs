using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Actions.Selects
{
    public sealed class SelectByValueStrategy : ISelectStrategy<string>
    {
        public void Select(SelectElement selectElement, string value) => selectElement.SelectByValue(value);
    }

    public sealed class SelectByIndexStrategy : ISelectStrategy<int>
    {
        public void Select(SelectElement selectElement, int value) => selectElement.SelectByIndex(value);
    }

    public sealed class SelectByTextStrategy : ISelectStrategy<string>
    {
        public void Select(SelectElement selectElement, string value) => selectElement.SelectByText(value);
    }
}
