using Xunit;

namespace Tranquire.Selenium.Tests;

public abstract class WebDriverTest : IClassFixture<WebDriverFixture>
{
    protected readonly WebDriverFixture Fixture;
    private readonly string _page;

    protected WebDriverTest(WebDriverFixture fixture, string page)
    {
        if (string.IsNullOrEmpty(page))
        {
            throw new System.ArgumentNullException(nameof(page));
        }

        Fixture = fixture;
        _page = page;
        ReloadPage();
    }

    protected T Answer<T>(IQuestion<T> question)
    {
        return Fixture.Actor.AsksFor(question);
    }

    public void ReloadPage()
    {
        Fixture.NavigateTo(_page);
    }
}
