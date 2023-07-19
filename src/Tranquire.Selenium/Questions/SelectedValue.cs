using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Questions;

/// <summary>
/// Allow to ask questions about the selected value of a select element
/// </summary>
public class SelectedValue : SingleUIState<string, SelectedValue>
{
    /// <summary>
    /// Creates a new instance of <see cref="SelectedValue"/>
    /// </summary>
    /// <param name="target"></param>
    public SelectedValue(ITarget target) : this(target, CultureInfo.CurrentCulture)
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="SelectedValue"/> with a culture
    /// </summary>
    /// <param name="target"></param>
    /// <param name="culture"></param>
    public SelectedValue(ITarget target, CultureInfo culture) : base(target, culture)
    {
    }

    /// <summary>
    /// Ask questions about the selected value of a select element
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static SelectedValue Of(ITarget target)
    {
        return new SelectedValue(target);
    }

    /// <summary>
    /// Creates a new instance of <see cref="SelectedValue"/>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    protected override SelectedValue CreateState(ITarget target, CultureInfo culture)
    {
        return new SelectedValue(target, culture);
    }

    /// <summary>
    /// Returns the selected value of the given element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    protected override string ResolveFor(IWebElement element)
    {
        var selected = new SelectElement(element);
        if (selected.AllSelectedOptions.Count == 0)
        {
            return string.Empty;
        }
        return selected.SelectedOption.GetAttribute("value");
    }
}
