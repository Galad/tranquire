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
    public partial class Questions 
    {
        [Theory]
        [InlineData("PresentElement", true)]
        [InlineData("NotPresentElement", false)]
        public void Presence_ShouldReturnCorrectValue(string id, bool expected)
        {
            //arrange
            var target = Target.The("presence element").LocatedBy(By.Id(id));
            var question = Presence.Of(target).Value;
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
