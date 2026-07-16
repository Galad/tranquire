using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions
{
    public class WindowTests : WebDriverTest
    {
        public WindowTests(WebDriverFixture fixture) : base(fixture, "Window.html")
        {

        }

        [Fact]
        public void ResizeTest()
        {
            var size = Fixture.WebDriver.Manage().Window.Size;
            try
            {
                var expected = new Size(516, 560);
                Fixture.Actor.When(WebBrowserWindow.Resize(expected));
                var actual = Fixture.WebDriver.Manage().Window.Size;
                Assert.Equal(expected, actual);
            }
            finally
            {
                Fixture.WebDriver.Manage().Window.Size = size;
            }
        }

        [Fact]
        public void MaximizeTest()
        {
            var restoreSize = Fixture.WebDriver.Manage().Window.Size;
            try
            {
                var size = new Size(400, 400);
                Fixture.Actor.Given(WebBrowserWindow.Resize(size));
                Fixture.Actor.When(WebBrowserWindow.Maximize());
                var actual = Fixture.WebDriver.Manage().Window.Size;
                Assert.True(actual.Width > size.Width && actual.Height > size.Height, $"Actual: {actual}; Previous: {size}");
                Assert.Equal(new Point(-8, -8), Fixture.WebDriver.Manage().Window.Position);
            }
            finally
            {
                Fixture.WebDriver.Manage().Window.Size = restoreSize;
            }
        }

        [Fact]
        public void MinimizeTest()
        {
            var restoreSize = Fixture.WebDriver.Manage().Window.Size;
            try
            {
                var size = new Size(400, 400);
                Fixture.Actor.Given(WebBrowserWindow.Resize(size));
                Fixture.Actor.When(WebBrowserWindow.Minimize());
                var actual = Fixture.WebDriver.Manage().Window.Size;
                //Assert.True(actual.Width == size.Width && actual.Height == size.Height, $"Actual: {actual}; Previous: {size}");
                Assert.Equal(new Point(10, 10), Fixture.WebDriver.Manage().Window.Position);
            }
            finally
            {
                Fixture.WebDriver.Manage().Window.Size = restoreSize;
            }
        }

        [Fact]
        public void FullScreenTest()
        {
            var restoreSize = Fixture.WebDriver.Manage().Window.Size;
            try
            {
                var size = new Size(400, 400);
                Fixture.Actor.Given(WebBrowserWindow.Resize(size));
                Fixture.Actor.When(WebBrowserWindow.FullScreen());
                var actual = Fixture.WebDriver.Manage().Window.Size;
                Assert.True(actual.Width > size.Width && actual.Height > size.Height, $"Actual: {actual}; Previous: {size}");
                Assert.Equal(Point.Empty, Fixture.WebDriver.Manage().Window.Position);
            }
            finally
            {
                Fixture.WebDriver.Manage().Window.Size = restoreSize;
            }
        }
    }
}
