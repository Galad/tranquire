using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tranquire.Selenium.Extensions;
using Xunit;

namespace Tranquire.Selenium.Tests.Extensions
{
    public class ScreenshotFormatTests
    {
        public static IEnumerable<object[]> FormatsAndValues
        {
            get
            {
                return new[]
                {
                    new object[]{ ScreenshotFormat.Bmp, ScreenshotImageFormat.Bmp, ".bmp"},
                    new object[]{ ScreenshotFormat.Jpeg, ScreenshotImageFormat.Jpeg, ".jpeg"},
                    new object[]{ ScreenshotFormat.Tiff, ScreenshotImageFormat.Tiff, ".tiff"},
                    new object[]{ ScreenshotFormat.Png, ScreenshotImageFormat.Png, ".png"},
                    new object[]{ ScreenshotFormat.Gif, ScreenshotImageFormat.Gif, ".gif"},
                };
            }
        }

        public static IEnumerable<object[]> Formats => FormatsAndValues.Select(f => new object[] { f[0] });

        [Theory, MemberData(nameof(FormatsAndValues))]
        public void ScreenshotFormat_ShouldHaveCorrectValue(ScreenshotFormat sut, ScreenshotImageFormat expectedFormat, string expectedExtension)
        {
            Assert.Equal(expectedFormat, sut.Format);
            Assert.Equal(expectedExtension, sut.Extension);
        }

        [Theory, MemberData(nameof(Formats))]
        public void GetHashCode_WithSameFormat_ReturnSameValue(ScreenshotFormat sut)
        {
            Assert.Equal(sut.GetHashCode(), sut.GetHashCode());
        }

        [Theory, MemberData(nameof(Formats))]
        public void GetHashCode_WithDifferentFormat_ReturnDifferentValue(ScreenshotFormat sut)
        {
            if(sut.Format == ScreenshotImageFormat.Bmp)
            {
                return;
            }

            Assert.NotEqual(ScreenshotFormat.Bmp.GetHashCode(), sut.GetHashCode());
        }
        
        [Theory, MemberData(nameof(Formats))]
        public void Equals_WithSameFormat_ReturnsTrue(ScreenshotFormat sut)
        {
            Assert.True(((object)sut).Equals(sut));
        }
        
        [Theory, MemberData(nameof(Formats))]
        public void Equals_WithDifferentFormat_ReturnsFalse(ScreenshotFormat sut)
        {
            if (sut.Format == ScreenshotImageFormat.Bmp)
            {
                return;
            }

            Assert.False(((object)ScreenshotFormat.Bmp).Equals(sut));
        }
    }
}
