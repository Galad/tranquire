using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using Tranquire.Selenium.Questions;

namespace Tranquire.Selenium.Actions.UIModels;
internal static class UpdateField
{
    private static readonly Dictionary<string, string> DateInputFormats = new()
    {
        ["date"] = "yyyy-MM-dd",
        ["datetime-local"] = "yyyy-MM-dd'T'HH:mm",
        ["time"] = "HH:mm"
    };

    public static void PerformFieldAction(IActor actor, FormField field)
    {
        switch (field.TagName)
        {
            case "input":
                HandleInputField(actor, field);
                break;
            case "select":
                if (field.Value is IEnumerable values && field.Value is not string)
                {
                    var options = values.Cast<object>().Select(value => FormatValue(value, field.Culture)).ToArray();
                    actor.Execute(Select.TheValues(options).Into(field.Target));
                }
                else
                {
                    actor.Execute(Select.TheValue(FormatValue(field.Value, field.Culture)).Into(field.Target));
                }
                break;
            case "textarea":
                EnterValue(actor, field, FormatValue(field.Value, field.Culture));
                break;
            default:
                throw new NotSupportedException(
                    $"Cannot update '{field.Key}': <{field.TagName}> elements are not supported. " +
                    $"Use a custom {nameof(BaseFormModelAttribute)} to update it.");
        }
    }

    private static string FormatValue(object value, CultureInfo culture) =>
        value is IFormattable formattable
            ? formattable.ToString(null, culture)
            : value?.ToString();

    private static void EnterValue(IActor actor, FormField field, string text)
    {
        actor.Execute(Enter.TheNewValue(text).Into(field.Target));
    }

    private static void HandleInputField(IActor actor, FormField field)
    {
        switch (field.Type)
        {
            case "checkbox":
                HandleCheckbox(actor, field);
                break;
            case "radio":
                HandleRadio(actor, field);
                break;
            // HTML number inputs only accept invariant numbers.
            case "number":
                EnterValue(actor, field, FormatValue(field.Value, CultureInfo.InvariantCulture));
                break;
            case "text":
            case "password":
            case "email":
            case "search":
            case "tel":
            case "url":
                HandleTextualInput(actor, field);
                break;
            case "date":
            case "datetime-local":
            case "time":
                HandleDateInput(actor, field);
                break;
            default:
                EnterValue(actor, field, FormatValue(field.Value, field.Culture));
                break;
        }
    }

    private static void HandleCheckbox(IActor actor, FormField field)
    {
        if (field.Value is bool selected && selected != actor.AsksFor(Selected.Of(field.Target)))
        {
            actor.Execute(Click.On(field.Target));
        }
    }

    private static void HandleRadio(IActor actor, FormField field)
    {
        var radioValue = FormatValue(field.Value, field.Culture);
        var radioGroupName = actor.AsksFor(HtmlAttribute.Of(field.Target).Named("name"));
        if (string.IsNullOrEmpty(radioGroupName))
        {
            radioGroupName = actor.AsksFor(HtmlAttribute.Of(field.Target).Named("id"));
        }
        var radioGroupNameLiteral = XPathStringLiteral.Format(radioGroupName);
        var radioValueLiteral = XPathStringLiteral.Format(radioValue);
        By radioBy = !string.IsNullOrEmpty(radioGroupName)
            ? By.XPath($".//input[@type='radio' and (@name={radioGroupNameLiteral} or @id={radioGroupNameLiteral}) and @value={radioValueLiteral}]")
            : By.XPath($".//input[@type='radio' and @value={radioValueLiteral}]");
        var radioTarget = Target.The($"{field.Key} radio with value {radioValue}")
            .LocatedBy(radioBy)
            .RelativeTo(field.Container);
        actor.Execute(Click.On(radioTarget));
    }

    private static void HandleTextualInput(IActor actor, FormField field)
    {
        var placeholder = actor.AsksFor(HtmlAttribute.Of(field.Target).Named("placeholder"));
        EnterValue(actor, field, FormatTextualValue(field, placeholder));
    }

    private static string FormatTextualValue(FormField field, string placeholder)
    {
        var format = DateTimeFormatter.MapPlaceholderToDotNetFormat(placeholder);
        if (format == null || field.Value is not (DateTime or DateTimeOffset))
            return FormatValue(field.Value, field.Culture);

        try
        {
            // The placeholder spells out its separators, so they must not be replaced by the culture's.
            return ((IFormattable)field.Value).ToString(format, CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            return FormatValue(field.Value, field.Culture);
        }
    }

    // Typing into date/time inputs depends on the browser locale, so the ISO value is set directly.
    private static void HandleDateInput(IActor actor, FormField field)
    {
        var text = field.Value is DateTime or DateTimeOffset
            ? ((IFormattable)field.Value).ToString(DateInputFormats[field.Type], CultureInfo.InvariantCulture)
            : FormatValue(field.Value, CultureInfo.InvariantCulture);
        actor.Execute(new SetInputValue(field.Target, text));
    }
}
