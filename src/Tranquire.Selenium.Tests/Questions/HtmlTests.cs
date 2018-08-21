using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
        public async Task HtmlOfPage_ShouldReturnCorrectValue()
        {
            // arrange
            var expected = normalizeHtml(await GetExpectedHtml()); 
            // act
            var actual = Answer(Html.OfPage);
            // assert
            Assert.Equal(expected, normalizeHtml(actual));

            string normalizeHtml(string html)
            {
                return html.Replace("\r\n", string.Empty)
                           .Replace("\n", string.Empty)
                           .Replace("\r", string.Empty);
            }
        }

        private async Task<string> GetExpectedHtml()
        {
            using(var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tranquire.Selenium.Tests.Html.html"))
            {
                var sr = new StreamReader(stream);
                return await sr.ReadToEndAsync();
            }
        }
    }
}
