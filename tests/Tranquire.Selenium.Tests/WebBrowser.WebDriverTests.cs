using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OpenQA.Selenium;
using Xunit;

namespace Tranquire.Selenium.Tests;

public class WebBrowserWebDriverTests
{
    private readonly Mock<IWebDriver> _driverMock;
    private readonly WebBrowser _sut;

    public WebBrowserWebDriverTests()
    {
        _driverMock = new Mock<IWebDriver>();
        _sut = new WebBrowser(_driverMock.Object);
    }

    [Fact]
    public void Close_ShouldCallDriverClose()
    {
        // act
        _sut.Close();
        // assert
        _driverMock.Verify(d => d.Close(), Times.Once());
    }

    [Fact]
    public async Task DisposeAsync_ShouldCallDriverDisposeAsync()
    {
        // arrange
        var driverMock = new Mock<IWebDriver>();
        driverMock.As<IAsyncDisposable>()
                  .Setup(d => d.DisposeAsync())
                  .Returns(new ValueTask());
        var sut = new WebBrowser(driverMock.Object);
        // act
        await sut.DisposeAsync();
        // assert
        driverMock.As<IAsyncDisposable>().Verify(d => d.DisposeAsync(), Times.Once());
    }

    [Fact]
    public void Manage_ShouldReturnDriverManageResult()
    {
        // arrange
        var optionsMock = new Mock<IOptions>();
        _driverMock.Setup(d => d.Manage()).Returns(optionsMock.Object);
        // act
        var result = _sut.Manage();
        // assert
        result.Should().Be(optionsMock.Object);
    }

    [Fact]
    public void Quit_ShouldCallDriverQuit()
    {
        // act
        _sut.Quit();
        // assert
        _driverMock.Verify(d => d.Quit(), Times.Once());
    }
}
