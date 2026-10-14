using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using Tranquire.Selenium.Questions;

namespace Tranquire.Selenium.Actions.UIModels;
internal static class UpdateField
{
    public static void PerformFieldAction(
        IActor actor,
        string fieldKey,
        object value,
        string tagName,
        string type,
        ITarget fieldTarget,
        ITarget container)
    {
        switch (tagName)
        {
            case "input":
                HandleInputField(actor, fieldKey, value, type, fieldTarget, container);
                break;
            case "select":
                if (value is IEnumerable<string> values)
                {
                    actor.Execute(Select.TheValues(values).Into(fieldTarget));
                }
                else
                {
                    actor.Execute(Select.TheValue(value?.ToString()).Into(fieldTarget));
                }
                break;
            case "textarea":
                actor.Execute(Enter.TheValue(value?.ToString()).Into(fieldTarget));
                break;
        }
    }

    private static void HandleInputField(
        IActor actor,
        string fieldKey,
        object value,
        string type,
        ITarget fieldTarget,
        ITarget container)
    {
        switch (type)
        {
            case "checkbox":
                HandleCheckbox(actor, value, fieldTarget);
                break;
            case "radio":
                HandleRadio(actor, fieldKey, value, fieldTarget, container);
                break;
            case "text":
            case "password":
            case "email":
            case "number":
            case "search":
            case "tel":
            case "url":
                HandleTextualInput(actor, value, fieldTarget);
                break;
            default:
                actor.Execute(Enter.TheNewValue(value?.ToString()).Into(fieldTarget));
                break;
        }
    }

    private static void HandleCheckbox(IActor actor, object value, ITarget fieldTarget)
    {
        if (value is bool b)
        {
            var isChecked = actor.AsksFor(HtmlAttribute.Of(fieldTarget).Named("checked")) == "true";
            if (b != isChecked)
            {
                actor.Execute(Click.On(fieldTarget));
            }
        }
    }

    private static void HandleRadio(
        IActor actor,
        string fieldKey,
        object value,
        ITarget fieldTarget,
        ITarget container)
    {
        var radioValue = value?.ToString();
        var radioGroupName = actor.AsksFor(HtmlAttribute.Of(fieldTarget).Named("name"));
        if (string.IsNullOrEmpty(radioGroupName))
        {
            radioGroupName = actor.AsksFor(HtmlAttribute.Of(fieldTarget).Named("id"));
        }
        By radioBy = !string.IsNullOrEmpty(radioGroupName)
            ? By.XPath($".//input[@type='radio' and (@name='{radioGroupName}' or @id='{radioGroupName}') and @value='{radioValue}']")
            : By.XPath($".//input[@type='radio' and @value='{radioValue}']");
        var radioTarget = Target.The($"{fieldKey} radio with value {radioValue}")
            .LocatedBy(radioBy)
            .RelativeTo(container);
        actor.Execute(Click.On(radioTarget));
    }

    private static void HandleTextualInput(IActor actor, object value, ITarget fieldTarget)
    {
        var placeholder = actor.AsksFor(HtmlAttribute.Of(fieldTarget).Named("placeholder"));
        string formattedValue;
        if (!string.IsNullOrEmpty(placeholder) && value is IFormattable formattable)
        {
            var dotNetFormat = DateTimeFormatter.MapPlaceholderToDotNetFormat(placeholder);
            try
            {
                formattedValue = formattable.ToString(dotNetFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                formattedValue = formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        else
        {
            formattedValue = value?.ToString();
        }
        actor.Execute(Enter.TheNewValue(formattedValue).Into(fieldTarget));
    }
}
