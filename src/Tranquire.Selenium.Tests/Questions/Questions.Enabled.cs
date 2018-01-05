using OpenQA.Selenium;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public partial class QuestionsTests
    {
        [Theory]
        [InlineData("EnabledElement", true)]
        [InlineData("DisabledElement", false)]
        [InlineData("CannotBeDisabledElement", true)]
        public void EnabledElement_ShouldReturnCorrectValue(string id, bool expected)
        {
            //arrange
            var target = Target.The("enabled element").LocatedBy(By.Id(id));
            var question = Enabled.Of(target).Value;
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
