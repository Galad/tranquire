using System;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public class StringConverter : Converter<string>
    {
        public StringConverter()
            : base(
                  new GenericConverter<string, string>(s => s),
                  new GenericConverter<string, int>(int.Parse),
                  new GenericConverter<string, bool>(bool.Parse),
                  new GenericConverter<string, DateTime>(DateTime.Parse)
                  )
        {
        }
    }
}