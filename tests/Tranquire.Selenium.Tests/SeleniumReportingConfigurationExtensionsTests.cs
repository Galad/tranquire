using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tranquire.Selenium;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class SeleniumReportingConfigurationExtensionsTests
    {
        public static IEnumerable<object[]> FormatMethods
        {
            get
            {
                return typeof(ScreenshotFormat).GetProperties(BindingFlags.Public | BindingFlags.Static)
                                               .Where(p => p.PropertyType == typeof(ScreenshotFormat))
                                               .Select(p => GetParameters(p));

                object[] GetParameters(PropertyInfo p)
                {
                    var format = (ScreenshotFormat)p.GetValue(null);
                    var method = typeof(SeleniumReportingConfigurationExtensions).GetMethod("WithScreenshot" + format.Format.ToString());
                    return new object[] { format, method };
                }
            }
        }

        [Theory, MemberData(nameof(FormatMethods))]
        public void WithScreenshotMethods_ShouldSetTheCorrectFormat(ScreenshotFormat format, MethodInfo method)
        {
            // arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var configuration = fixture.Create<SeleniumReportingConfiguration>();

            // act
            var actual = (SeleniumReportingConfiguration)method.Invoke(null, new object[] { configuration });

            // assert
            Assert.Equal(format, actual.ScreenshotFormat);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(SeleniumReportingConfigurationExtensions));
        }
    }
}
