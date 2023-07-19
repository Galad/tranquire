using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Actions.Selects;

/// <summary>
/// A strategy that is used to select a value on a <see cref="SelectElement"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISelectStrategy<in T>
{
    /// <summary>
    /// Select the element
    /// </summary>
    /// <param name="selectElement">A <see cref="SelectElement"/> instance representing the HTML element</param>
    /// <param name="value">The value to select</param>
    void Select(SelectElement selectElement, T value);
}