using System;
using OpenQA.Selenium;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions
{
    public class Actions : WebDriverTest
    {
        private readonly ITarget SelectElementTarget = Target.The("select element").LocatedBy(By.Id("SelectElement"));
        private readonly ITarget SelectManyElementTarget = Target.The("select element").LocatedBy(By.Id("SelectElementMultiple"));
        public Actions(WebDriverFixture fixture) : base(fixture, "Actions.html")
        {

        }

        [Theory]
        [InlineData("LinkToClick")]
        [InlineData("ButtonToClick")]
        public void Click_ShouldExecuteClick(string buttonId)
        {
            TestClick(buttonId, Click.On);
        }

        [Theory]
        [InlineData("LinkToClick")]
        [InlineData("ButtonToClick")]
        public void JsClick_ShouldExecuteClick(string buttonId)
        {
            TestClick(buttonId, JsClick.On);
        }

        private void TestClick(string buttonId, Func<ITarget, IAction<Unit>> click)
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var inputTarget = Target.The("click input").LocatedBy(By.Id("ClickInput"));
            var clickTarget = Target.The("click target").LocatedBy(By.Id(buttonId));
            var expectedClickContent = Target.The("expected click content").LocatedBy(By.Id("ClickExpectedContent"));
            var expected = Guid.NewGuid().ToString();
            Fixture.Actor.Given(Enter.TheValue(expected).Into(inputTarget));
            //act            
            Fixture.Actor.When(click(clickTarget));
            //assert
            var actual = Answer(TextContent.Of(expectedClickContent));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("3")]
        [InlineData("2")]
        public void SelectByValueAction_ShouldSelectCorrectElement(string expected)
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var action = Select.TheValue(expected).Into(SelectElementTarget);
            //act
            Fixture.Actor.When(action);
            var actual = Answer(SelectedValue.Of(SelectElementTarget));
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, new string[] { })]
        [InlineData(1, new string[] { "1" })]
        [InlineData(1, new string[] { "1", "2", "3" })]
        [InlineData(1, new string[] { "1", "3" })]
        public void SelectByValuesAction_ShouldSelectCorrectElement(
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
            int dummy,
            string[] expected)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var action = Select.TheValues(expected).Into(SelectManyElementTarget);
            //act
            Fixture.Actor.When(action);
            var actual = Answer(SelectedValues.Of(SelectManyElementTarget));
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "1")]
        [InlineData(2, "3")]
        [InlineData(1, "2")]
        public void SelectByIndexAction_ShouldSelectCorrectElement(int index, string expected)
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var action = Select.TheIndex(index).Into(SelectElementTarget);
            //act
            Fixture.Actor.When(action);
            var actual = Answer(SelectedValue.Of(SelectElementTarget));
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new int[] { }, new string[] { })]
        [InlineData(new int[] { 0 }, new string[] { "1" })]
        [InlineData(new int[] { 0, 1, 2 }, new string[] { "1", "2", "3" })]
        [InlineData(new int[] { 0, 2 }, new string[] { "1", "3" })]
        public void SelectByIndexesAction_ShouldSelectCorrectElement(int[] indexes, string[] expected)
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var action = Select.TheIndexes(indexes).Into(SelectManyElementTarget);
            //act
            Fixture.Actor.When(action);
            var actual = Answer(SelectedValues.Of(SelectManyElementTarget));
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Yellow", "1")]
        [InlineData("Red", "3")]
        [InlineData("Green", "2")]
        public void SelectByTextAction_ShouldSelectCorrectElement(string text, string expected)
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var action = Select.TheText(text).Into(SelectElementTarget);
            //act
            Fixture.Actor.When(action);
            var actual = Answer(SelectedValue.Of(SelectElementTarget));
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new string[] { }, new string[] { })]
        [InlineData(new string[] { "Yellow" }, new string[] { "1" })]
        [InlineData(new string[] { "Yellow", "Green", "Red" }, new string[] { "1", "2", "3" })]
        [InlineData(new string[] { "Yellow", "Red" }, new string[] { "1", "3" })]
        public void SelectByTextsAction_ShouldSelectCorrectElement(string[] texts, string[] expected)
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var action = Select.TheTexts(texts).Into(SelectManyElementTarget);
            //act
            Fixture.Actor.When(action);
            var actual = Answer(SelectedValues.Of(SelectManyElementTarget));
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaa")]
        [InlineData("very long text aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void ClearValue_ShouldClearTheValue(string inputValue)
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var target = Target.The("input to clear").LocatedBy(By.Id("InputToClear"));
            Fixture.Actor.When(Enter.TheValue(inputValue).Into(target));
            //act
            Fixture.Actor.When(Clear.TheValueOf(target));
            //assert
            var actual = Answer(Value.Of(target));
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ClearValue_WhenCursorIsNotAtTheEnd_ShouldClearTheValue()
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var target = Target.The("input to clear").LocatedBy(By.Id("InputToClear"));
            Fixture.Actor.When(Enter.TheValue("Value to clear").Into(target));
            Fixture.Actor.When(Enter.TheValue(Keys.ArrowLeft).Into(target));
            //act
            Fixture.Actor.When(Clear.TheValueOf(target));
            //assert
            var actual = Answer(Value.Of(target));
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ClearValue_WhenCursorIsAtTheBeginning_ShouldClearTheValue()
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var target = Target.The("input to clear").LocatedBy(By.Id("InputToClear"));
            Fixture.Actor.When(Enter.TheValue("Value to clear").Into(target));
            Fixture.Actor.When(Enter.TheValue(Keys.Home).Into(target));
            //act
            Fixture.Actor.When(Clear.TheValueOf(target));
            //assert
            var actual = Answer(Value.Of(target));
            Assert.Equal(string.Empty, actual);
        }
    }
}
