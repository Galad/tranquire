using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public partial class QuestionsTests
    {
        [Theory]
		[InlineData("ValueElement", "the value")]
        [InlineData("EmptyValueElement", "")]
        [InlineData("NoValueElement", "")]
        [InlineData("OptionValueElement", "1")]
        [InlineData("OptionNoValueElement", "Green")]
        public void ValueElement_ShouldReturnCorrectValue(string id, string expected)
        {
            //arrange
            var target = Target.The("value element").LocatedBy(By.Id(id));
            var question = Value.Of(target).Value;
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
