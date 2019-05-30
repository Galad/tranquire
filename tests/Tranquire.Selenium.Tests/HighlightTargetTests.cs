using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Tranquire;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class HighlightTargetTests
    {
        public static IEnumerable<object[]> SystemColors
        {
            get
            {
                return typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                                    .Where(p => p.PropertyType == typeof(Color))
                                    .Select(p => new object[] { p.GetValue(null), p.Name == "LightGray" ? "LightGrey" : p.Name.ToString() });
            }
        }

        [Theory, MemberData(nameof(SystemColors))]
        public void ToHtml_NamedColor_ShouldReturnCorrectValue(Color color, string expected)
        {
            // act
            var actual = HighlightTarget.ToHtml(color);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(131, 67, 140, "#83438C")]
        [InlineData(129, 131, 201, "#8183C9")]
        [InlineData(178, 217, 158, "#B2D99E")]
        public void ToHtml_Other_ShouldReturnCorrectValue(int r, int g, int b, string expected)
        {
            // arrange
            var color = Color.FromArgb(r, g, b);
            // act
            var actual = HighlightTarget.ToHtml(color);
            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToHtml_EmptyColor_ShouldReturnCorrectValue()
        {
            // arrange
            var color = Color.Empty;
            // act
            var actual = HighlightTarget.ToHtml(color);
            // assert
            Assert.Equal(string.Empty, actual);
        }
    }
}
