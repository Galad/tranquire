using System;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions.UIModels;

internal static class ByMethodExtensions
{
    public static By ToBy(this ByMethod byMethod, string value)
    {
        switch (byMethod)
        {
            case ByMethod.XPath:
                return By.XPath(value);
            case ByMethod.CssSelector:
                return By.CssSelector(value);
            case ByMethod.Id:
                return By.Id(value);
            case ByMethod.Name:
                return By.Name(value);
            case ByMethod.ClassName:
                return By.ClassName(value);
            case ByMethod.TagName:
                return By.TagName(value);
            default:
                throw new NotSupportedException($"Unsupported ByMethod: {byMethod}");
        }
    }
}
