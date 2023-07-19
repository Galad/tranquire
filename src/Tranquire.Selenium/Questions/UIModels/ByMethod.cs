using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions.UIModels;

/// <summary>
/// Indicates the method for <see cref="By"/>
/// </summary>
public enum ByMethod
{
    /// <summary>
    /// The <see cref="By.CssSelector(string)"/> method
    /// </summary>
    CssSelector,
    /// <summary>
    /// The <see cref="By.Id(string)"/> method
    /// </summary>
    Id,
    /// <summary>
    /// The <see cref="By.Name(string)"/> method
    /// </summary>
    Name,
    /// <summary>
    /// The <see cref="By.ClassName(string)"/> method
    /// </summary>
    ClassName,
    /// <summary>
    /// The <see cref="By.TagName(string)"/> method
    /// </summary>
    TagName,
    /// <summary>
    /// The <see cref="By.XPath(string)"/> method
    /// </summary>
    XPath,
    /// <summary>
    /// Use the container element
    /// </summary>
    Self
}
