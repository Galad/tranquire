using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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
        [InlineData("SelectedElement", true)]
        [InlineData("NotSelectedElement", false)]
        public void SelectedElement_ShouldReturnCorrectValue(string id, bool expected)
        {
            //arrange
            var target = Target.The("selected element").LocatedBy(By.Id(id));
            var question = Selected.Of(target).Value;
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("SelectedValue", "2")]
        [InlineData("SelectedNoValue", "Some other option")]
        [InlineData("NoSelectedValue", "1")]
        [InlineData("SelectedValueMultiple", "1")]
        [InlineData("NoSelectedValueMultiple", "")]
        public void SelectedValueElement_ShouldReturnCorrectValue(string id, string expected)
        {
            //arrange
            var target = Target.The("selected element").LocatedBy(By.Id(id));
            var question = SelectedValue.Of(target).Value;
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SelectedValueElement_WhenTargetIsNotASelectElement_ShouldThrow()
        {
            //arrange
            var target = Target.The("selected element").LocatedBy(By.Id("NotASelectElement"));
            var question = SelectedValue.Of(target).Value;
            //act
            Assert.Throws<UnexpectedTagNameException>(() => Answer(question));
        }
    }
}
