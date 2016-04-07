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
        [InlineData("ClassesElement1", new[] { "highlighted" })]
        [InlineData("ClassesElement2", new[] { "highlighted", "otherclass1", "otherclass2" })]
        [InlineData("ClassesElement3", new[] { "highlighted", "otherclass1", "otherclass2" })]
        [InlineData("ClassesElement4", new string[] { })]
        [InlineData("ClassesElement5", new string[] { })]
        public void Classes_ShouldReturnCorrectValue(string id, string[] expected)
        {
            //arrange
            var target = Target.The("Element with classes").LocatedBy(By.Id(id));
            var question = Classes.Of(target).Value;
            //act            
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
