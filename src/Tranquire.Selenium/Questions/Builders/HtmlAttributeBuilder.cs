namespace Tranquire.Selenium.Questions.Builders;

/// <summary>
/// Build a <see cref="HtmlAttribute "/> object
/// </summary>
public class HtmlAttributeBuilder
{
    private readonly ITarget _target;

    /// <summary>
    /// Creates a new instance of <see cref="HtmlAttributeBuilder"/>
    /// </summary>
    /// <param name="target"></param>
    public HtmlAttributeBuilder(ITarget target)
    {
        _target = target;
    }

    /// <summary>
    /// Returns a <see cref="HtmlAttribute"/> object with the given attribute name
    /// </summary>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public HtmlAttribute Named(string attributeName)
    {
        return new HtmlAttribute(_target, attributeName);
    }
}
