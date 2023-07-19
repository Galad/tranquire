using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions;

public partial class QuestionsTests
{
    [Theory]
    [InlineData("SelectedElement", true)]
    [InlineData("NotSelectedElement", false)]
    public void SelectedElement_ShouldReturnCorrectValue(string id, bool expected)
    {
        //arrange
        var target = Target.The("selected element").LocatedBy(By.Id(id));
        var question = Selected.Of(target);
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
        var question = SelectedValue.Of(target);
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
        var question = SelectedValue.Of(target);
        //act
        Assert.Throws<UnexpectedTagNameException>(() => Answer(question));
    }


    [Theory]
    [InlineData("SelectedValues", new[] { "2" })]
    [InlineData("SelectedNoValues", new[] { "Some other option" })]
    [InlineData("NoSelectedValues", new string[] { "1" })]
    [InlineData("SelectedValuesMultiple", new[] { "1", "2" })]
    [InlineData("NoSelectedValuesMultiple", new string[] { })]
    public void SelectedValuesElement_ShouldReturnCorrectValue(string id, string[] expected)
    {
        //arrange
        var target = Target.The("selected element").LocatedBy(By.Id(id));
        var question = SelectedValues.Of(target);
        //act
        var actual = Answer(question);
        //assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectedValuesElement_WhenTargetIsNotASelectElement_ShouldThrow()
    {
        //arrange
        var target = Target.The("selected element").LocatedBy(By.Id("NotASelectElement"));
        var question = SelectedValues.Of(target);
        //act
        Assert.Throws<UnexpectedTagNameException>(() => Answer(question));
    }
}
