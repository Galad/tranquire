using OpenQA.Selenium;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public partial class QuestionsTests
    {
        [Theory]
        [InlineData("CssValueInlineStyle", "font-style", "italic")]
        [InlineData("CssValueInlineStyle", "noproperty", "")]
        [InlineData("CssValueClass", "font-style", "italic")]
        [InlineData("CssValueClass", "noproperty", "")]
        public void CssValueElement_ShouldReturnCorrectValue(string id, string cssProperty, string expected)
        {
            //arrange
            var target = Target.The("css value element").LocatedBy(By.Id(id));
            var question = CssValue.Of(target).AndTheProperty(cssProperty).Value;
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
