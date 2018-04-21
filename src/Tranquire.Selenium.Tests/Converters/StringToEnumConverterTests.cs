using System;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;
using Xunit;

namespace Tranquire.Selenium.Tests.Converters
{
    public class StringToEnumConverterTests
    {
        public enum TestEnum
        {
            Red,
            Blue,
            Green
        }

        private readonly StringToEnumConverter<TestEnum> _sut;

        public StringToEnumConverterTests()
        {
            _sut = new StringToEnumConverter<TestEnum>();
        }

        [Fact]
        public void Sut_ShouldBeConverter()
        {
            Assert.IsAssignableFrom<IConverter<string, TestEnum>>(_sut);
        }

        [Theory]
        [InlineData("Red", TestEnum.Red)]
        [InlineData("Green", TestEnum.Green)]
        [InlineData("Blue", TestEnum.Blue)]
        [InlineData("0", TestEnum.Red)]
        [InlineData("1", TestEnum.Blue)]
        [InlineData("2", TestEnum.Green)]
        public void Convert_ShouldReturnCorrectValue(string value, TestEnum expected)
        {
            //act
            var actual = _sut.Convert(value, CultureInfo.InvariantCulture);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("lklsdfj")]
        [InlineData("3")]
        public void Convert_WhenValueIsInvalid_ShouldThrow(string value)
        {
            //act and assert
            Assert.Throws<FormatException>(() => _sut.Convert(value, CultureInfo.InvariantCulture));
        }
    }
}
