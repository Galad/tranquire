using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Tranquire.Selenium.Questions.UIModels;
using Tranquire.Selenium.Targets;

namespace Tranquire.Selenium.Actions.UIModels;

internal static class ResolveTarget
{
    public static ITarget ResolveTargets(PropertyInfo property,
        object model, object valueToSet, string[] propertiesTargetParameters,
        TargetWithParametersAttribute attr,
        ITarget container)
    {
        var selector = ResolveFormat(
            attr.Format,
            model,
            valueToSet,
            attr.FormatParameters,
            propertiesTargetParameters);
        By by = ResolveBy(attr, selector);
        return Target.The(property.Name).LocatedBy(by).RelativeTo(container);
    }
    /// <summary>Resolves a class-level selector against the model instance.</summary>
    public static string ResolveClassLevelFormat(TargetWithParametersAttribute attr, object model)
    {
        return ResolveFormat(attr.Format, model, null, attr.FormatParameters, Array.Empty<string>());
    }

    /// <summary>
    /// Resolves the selector portion of a target attribute format string by substituting
    /// <c>{PropertyName}</c> placeholders with values from the model instance.
    /// </summary>
    public static string ResolveNamedFormat(string format, object model)
    {
        return ResolveFormat(format, model, null, Array.Empty<string>(), Array.Empty<string>());
    }

    private static string ResolveFormat(
        string format,
        object model,
        object currentValue,
        string[] formatParameters,
        string[] suppliedParameters)
    {
        formatParameters ??= Array.Empty<string>();
        suppliedParameters ??= Array.Empty<string>();

        var readableProperties = model.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanRead)
            .ToArray();
        var modelProperties = readableProperties.ToDictionary(
            property => property.Name,
            property => property.GetValue(model),
            StringComparer.Ordinal);
        var positionalModelValues = readableProperties.Select(property => property.GetValue(model)).ToArray();

        var namedParameters = CreateNamedParameters(
            formatParameters,
            suppliedParameters,
            currentValue,
            modelProperties,
            model.GetType());
        var positionalParameters = CreatePositionalParameters(
            formatParameters,
            suppliedParameters,
            currentValue,
            namedParameters,
            positionalModelValues);

        return Regex.Replace(format, @"\{(?<parameter>[A-Za-z_][A-Za-z0-9_]*|\d+)\}", match =>
            ResolvePlaceholder(match.Groups["parameter"].Value, positionalParameters, namedParameters, modelProperties, model.GetType()));
    }

    private static Dictionary<string, object> CreateNamedParameters(
        string[] formatParameters,
        string[] suppliedParameters,
        object currentValue,
        IDictionary<string, object> modelProperties,
        Type modelType)
    {
        var namedParameters = new Dictionary<string, object>(StringComparer.Ordinal);
        if (currentValue != null)
            namedParameters["Value"] = currentValue;

        for (var index = 0; index < formatParameters.Length; index++)
        {
            var parameterName = formatParameters[index];
            if (parameterName == "Value")
                namedParameters[parameterName] = currentValue;
            else if (index < suppliedParameters.Length)
                namedParameters[parameterName] = suppliedParameters[index];
            else if (modelProperties.TryGetValue(parameterName, out var propertyValue))
                namedParameters[parameterName] = propertyValue;
            else
                throw new InvalidOperationException(
                    $"Format parameter '{parameterName}' is not a readable property on '{modelType.Name}' and no value was supplied.");
        }

        return namedParameters;
    }

    private static object[] CreatePositionalParameters(
        string[] formatParameters,
        string[] suppliedParameters,
        object currentValue,
        IDictionary<string, object> namedParameters,
        object[] positionalModelValues)
    {
        if (suppliedParameters.Length > 0)
            return suppliedParameters.Cast<object>().ToArray();
        if (formatParameters.Length > 0)
            return formatParameters.Select(parameterName => namedParameters[parameterName]).ToArray();
        if (currentValue != null)
            return new[] { currentValue };
        return positionalModelValues;
    }

    private static string ResolvePlaceholder(
        string parameter,
        object[] positionalParameters,
        IDictionary<string, object> namedParameters,
        IDictionary<string, object> modelProperties,
        Type modelType)
    {
        if (!int.TryParse(parameter, out var index))
        {
            if (namedParameters.TryGetValue(parameter, out var namedValue) ||
                modelProperties.TryGetValue(parameter, out namedValue))
                return namedValue?.ToString() ?? string.Empty;

            throw new InvalidOperationException(
                $"Format placeholder '{{{parameter}}}' is not a readable property or declared format parameter on '{modelType.Name}'.");
        }

        if (index >= positionalParameters.Length)
            throw new InvalidOperationException(
                $"Format placeholder '{{{parameter}}}' has no corresponding parameter for '{modelType.Name}'.");

        return positionalParameters[index]?.ToString() ?? string.Empty;
    }

    public static By ResolveBy(TargetWithParametersAttribute attr, string selector)
    {
        if (attr.By != null)
        {
            return attr.By;
        }
        return ResolveBy(attr.ByMethod, selector);
    }

    public static By ResolveBy(ByMethod byMethod, string selector)
    {
        switch (byMethod)
        {
            case ByMethod.XPath:
                return By.XPath(selector);
            case ByMethod.CssSelector:
                return By.CssSelector(selector);
            case ByMethod.Id:
                return By.Id(selector);
            case ByMethod.Name:
                return By.Name(selector);
            case ByMethod.ClassName:
                return By.ClassName(selector);
            case ByMethod.TagName:
                return By.TagName(selector);
            default:
                throw new NotSupportedException($"Unsupported ByMethod: {byMethod}");
        }
    }
}
