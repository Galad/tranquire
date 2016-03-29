using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public partial class Questions
    {
        [Fact]
        public void PageTitle_ShouldReturnCorrectValue()
        {
            //arrange
            var question = Page.Title();
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal("The title", actual);
        }
    }
}
