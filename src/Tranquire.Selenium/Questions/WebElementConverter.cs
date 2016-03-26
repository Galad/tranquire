using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public class WebElementConverter : Converter<IWebElement>
    {
        public WebElementConverter(params ICanConvert<IWebElement>[] converters) : base(
                  converters.Concat(
                      new ICanConvert<IWebElement>[] 
                      {
                          new GenericConverter<IWebElement, string>(s => s.Text),
                          new GenericConverter<IWebElement, int>(s => int.Parse(s.Text)),
                          new GenericConverter<IWebElement, bool>(s => bool.Parse(s.Text)),
                          new GenericConverter<IWebElement, DateTime>(s => DateTime.Parse(s.Text))
                      }
                  ).ToArray())
        {
        }
    }
}
