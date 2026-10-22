using System.Globalization;

namespace Tranquire.Selenium.Actions.UIModels;

/// <summary>
/// Describes a form field being updated from a model property.
/// </summary>
public sealed class FormField
{
    internal FormField(
        string key,
        object value,
        string tagName,
        string type,
        ITarget target,
        ITarget container,
        CultureInfo culture)
    {
        Key = key;
        Value = value;
        TagName = tagName;
        Type = type;
        Target = target;
        Container = container;
        Culture = culture;
    }

    /// <summary>Gets the name of the model property being written.</summary>
    public string Key { get; }

    /// <summary>Gets the value to write.</summary>
    public object Value { get; }

    /// <summary>Gets the lower-case tag name of the element.</summary>
    public string TagName { get; }

    /// <summary>Gets the lower-case <c>type</c> attribute of the element, or null when it has none.</summary>
    public string Type { get; }

    /// <summary>Gets the target of the field.</summary>
    public ITarget Target { get; }

    /// <summary>Gets the container the model is updated in.</summary>
    public ITarget Container { get; }

    /// <summary>Gets the culture used to format values.</summary>
    public CultureInfo Culture { get; }
}
