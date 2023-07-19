namespace Tranquire.Selenium.Questions;

/// <summary>
/// A question returning the page title
/// </summary>
public class PageTitle : QuestionBase<WebBrowser, string>
{
    /// <summary>
    /// Returns the page title
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="ability"></param>
    /// <returns></returns>
    protected override string Answer(IActor actor, WebBrowser ability)
    {
        return ability.Driver.Title;
    }

    /// <summary>
    /// Gets the action's name
    /// </summary>
    public override string Name => "Page title";
}