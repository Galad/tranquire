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
