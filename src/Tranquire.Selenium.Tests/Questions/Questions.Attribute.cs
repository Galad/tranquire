using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public partial class QuestionsTests
    {
        [Theory]
        [InlineData("title", "The title")]
        [InlineData("other", default(string))]
        public void ElementWithAttribute_ShouldReturnCorrectValue(string attribute, string expected)
        {
            //arrange
            var target = Target.The("element with attribute").LocatedBy(By.Id("ElementWithAttribute"));
            var question = HtmlAttribute.Of(target).Named(attribute).Value;
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
