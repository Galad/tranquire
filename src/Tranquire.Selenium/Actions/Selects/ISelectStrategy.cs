using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Actions.Selects
{
    public interface ISelectStrategy<T>
    {        
        void Select(SelectElement selectElement, T value);
    }
}