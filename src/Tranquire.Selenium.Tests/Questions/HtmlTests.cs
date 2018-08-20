using System.IO;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public class HtmlTests : WebDriverTest
    {
        public HtmlTests(WebDriverFixture fixture) : base(fixture, "Html.html")
        {
        }

        [Fact]
        public void HtmlOfPage_ShouldReturnCorrectValue()
        {
            // arrange
            var expected = File.ReadAllText("Html.html").Replace("\r\n", string.Empty);
            // act
            var actual = Answer(Html.OfPage);
            // assert
            Assert.Equal(expected, actual.Replace("\r\n", string.Empty));
        }
    }
}
