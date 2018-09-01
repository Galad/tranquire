﻿using OpenQA.Selenium;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public partial class QuestionsTests
    {
        [Theory]
        [InlineData("VisibleElement", true)]
        [InlineData("NotDisplayedElement", false)]
        [InlineData("NotDisplayedElement2", false)]
        public void VisibleElement_ShouldReturnCorrectValue(string id, bool expected)
        {
            //arrange
            var target = Target.The("visible element").LocatedBy(By.Id(id));
            var question = Visibility.Of(target);
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
