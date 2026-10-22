using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Tranquire.Selenium.Questions.UIModels;
using Tranquire.Selenium.Targets;

namespace Tranquire.Selenium.Actions.UIModels;

internal static class ResolveTarget
{
    private const string CurrentValueParameter = "Value";
    private static readonly Regex PlaceholderPattern = new(
        @"\{(?<parameter>[A-Za-z_][A-Za-z0-9_]*|\d+)\}",
        RegexOptions.None,
        TimeSpan.FromSeconds(1));
    private static readonly Regex XPathFormatPattern = new(
        @"'[^']*'|""[^""]*""|(?<position>\[\s*" + PlaceholderPattern.ToString() +
        @"\s*\])|(?<name>/|@|::)?" + PlaceholderPattern.ToString(),
        RegexOptions.None,
        TimeSpan.FromSeconds(1));
    private static readonly Regex CssFormatPattern = new(
        @"'(?:[^'\\]|\\.)*'|""(?:[^""\\]|\\.)*""|" + PlaceholderPattern.ToString(),
        RegexOptions.None,
        TimeSpan.FromSeconds(1));
    private static readonly Regex CssTokenPattern = new(
        @"\A[A-Za-z0-9_\-\u0080-\uFFFF]*\z",
        RegexOptions.None,
        TimeSpan.FromSeconds(1));

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
            propertiesTargetParameters,
            attr.ByMethod);
        return Target.The(property.Name).LocatedBy(attr.ByMethod.ToBy(selector)).RelativeTo(container);
    }

    /// <summary>Creates the container target described by a class-level attribute for the model instance.</summary>
    public static ITarget ResolveClassTarget(TargetWithParametersAttribute attr, object model, string name)
    {
        return Target.The(name).LocatedBy(attr.ByMethod.ToBy(ResolveClassLevelFormat(attr, model)));
    }

    public static bool HasPlaceholders(string format) => format != null && PlaceholderPattern.IsMatch(format);

    /// <summary>Resolves a class-level selector against the model instance.</summary>
    public static string ResolveClassLevelFormat(TargetWithParametersAttribute attr, object model)
    {
        return ResolveFormat(
            attr.Format,
            model,
            null,
            attr.FormatParameters,
            Array.Empty<string>(),
            attr.ByMethod);
    }

    /// <summary>
    /// Resolves the selector portion of a target attribute format string by substituting
    /// <c>{PropertyName}</c> placeholders with values from the model instance.
    /// XPath and CSS values are escaped; unquoted names and numeric positions are validated.
    /// </summary>
    public static string ResolveNamedFormat(
        string format,
        object model,
        ByMethod? byMethod = null,
        object currentValue = null,
        string[] suppliedParameters = null)
    {
        return ResolveFormat(format, model, currentValue, Array.Empty<string>(), suppliedParameters, byMethod);
    }

    private static string ResolveFormat(
        string format,
        object model,
        object currentValue,
        string[] formatParameters,
        string[] suppliedParameters,
        ByMethod? byMethod)
    {
        suppliedParameters ??= Array.Empty<string>();

        var modelType = model.GetType();
        var modelProperties = modelType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanRead)
            .ToDictionary(
                property => property.Name,
                property => property.GetValue(model),
                StringComparer.Ordinal);

        var namedParameters = CreateNamedParameters(
            formatParameters,
            suppliedParameters,
            currentValue,
            modelProperties,
            modelType);
        var positionalParameters = CreatePositionalParameters(
            formatParameters,
            suppliedParameters,
            currentValue,
            namedParameters);

        string ResolvePlaceholders(string template, Func<string, string> encode) => PlaceholderPattern.Replace(
            template,
            match => encode(ResolvePlaceholder(
                match.Groups["parameter"].Value,
                positionalParameters,
                namedParameters,
                modelProperties,
                modelType)));

        string ResolveXPathMatch(Match match)
        {
            if (IsQuoted(match))
                return XPathStringLiteral.Format(ResolvePlaceholders(match.Value.Substring(1, match.Length - 2), Unencoded));

            if (match.Groups["position"].Success)
            {
                var position = ResolvePlaceholders(match.Value, Unencoded);
                _ = XmlConvert.ToDecimal(position.Substring(1, position.Length - 2));
                return position;
            }

            var name = match.Groups["name"];
            var value = ResolvePlaceholders(match.Value.Substring(name.Length), Unencoded);
            if (name.Success)
                return name.Value + (value == "*" ? value : VerifyXPathName(value));
            return XPathStringLiteral.Format(value);
        }

        string ResolveCssMatch(Match match)
        {
            if (IsQuoted(match))
                return ResolvePlaceholders(match.Value, EscapeCssString);
            return VerifyCssToken(ResolvePlaceholders(match.Value, Unencoded));
        }

        switch (byMethod)
        {
            case ByMethod.XPath:
                return XPathFormatPattern.Replace(format, ResolveXPathMatch);
            case ByMethod.CssSelector:
                return CssFormatPattern.Replace(format, ResolveCssMatch);
            default:
                return ResolvePlaceholders(format, Unencoded);
        }
    }

    private static bool IsQuoted(Match match) => match.Value[0] == '\'' || match.Value[0] == '"';

    private static string Unencoded(string value) => value;

    private static string EscapeCssString(string value)
    {
        var builder = new StringBuilder(value.Length);
        foreach (var character in value)
        {
            if (char.IsControl(character))
                builder.Append('\\').Append(((int)character).ToString("x", CultureInfo.InvariantCulture)).Append(' ');
            else if (character == '\\' || character == '\'' || character == '"')
                builder.Append('\\').Append(character);
            else
                builder.Append(character);
        }
        return builder.ToString();
    }

    private static string VerifyCssToken(string value)
    {
        if (!CssTokenPattern.IsMatch(value))
            throw new FormatException(
                $"'{value}' cannot be used unquoted in a CSS selector. Quote the placeholder to match arbitrary text.");
        return value;
    }

    private static string VerifyXPathName(string value)
    {
        var colonIndex = value.IndexOf(':');
        if (colonIndex >= 0)
            XmlConvert.VerifyNCName(value.Substring(0, colonIndex));
        XmlConvert.VerifyNCName(value.Substring(colonIndex + 1));
        return value;
    }

    private static Dictionary<string, object> CreateNamedParameters(
        string[] formatParameters,
        string[] suppliedParameters,
        object currentValue,
        IDictionary<string, object> modelProperties,
        Type modelType)
    {
        // A model property named Value takes precedence over the value being entered.
        var bindsCurrentValue = !modelProperties.ContainsKey(CurrentValueParameter);
        var namedParameters = new Dictionary<string, object>(StringComparer.Ordinal);
        if (currentValue != null && bindsCurrentValue)
            namedParameters[CurrentValueParameter] = currentValue;

        for (var index = 0; index < formatParameters.Length; index++)
        {
            var parameterName = formatParameters[index];
            if (parameterName == CurrentValueParameter && bindsCurrentValue)
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
        IDictionary<string, object> namedParameters)
    {
        var parameterCount = Math.Max(formatParameters.Length, suppliedParameters.Length);
        if (parameterCount > 0)
        {
            return Enumerable.Range(0, parameterCount)
                .Select(index => index < formatParameters.Length
                    ? namedParameters[formatParameters[index]]
                    : suppliedParameters[index])
                .ToArray();
        }
        return currentValue == null ? Array.Empty<object>() : new[] { currentValue };
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
                $"Format placeholder '{{{parameter}}}' has no corresponding parameter for '{modelType.Name}'. " +
                "Declare it in the format parameters or supply it when updating the model.");

        return positionalParameters[index]?.ToString() ?? string.Empty;
    }
}
