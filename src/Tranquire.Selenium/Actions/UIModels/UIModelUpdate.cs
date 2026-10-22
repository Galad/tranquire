using System;
using System.Globalization;

namespace Tranquire.Selenium.Actions.UIModels;

/// <summary>
/// An action that updates the UI from one or more models.
/// </summary>
public sealed class UIModelUpdate : ActionBaseUnit
{
    private readonly Action<IActor, CultureInfo> _update;
    private readonly CultureInfo _culture;

    internal UIModelUpdate(string name, Action<IActor, CultureInfo> update, CultureInfo culture = null)
    {
        Name = name;
        _update = update;
        _culture = culture ?? CultureInfo.CurrentCulture;
    }

    /// <inheritdoc />
    public override string Name { get; }

    /// <summary>
    /// Specify the culture used to format values, matching
    /// <see cref="Questions.UIModels.UIModel{T}.WithCulture(CultureInfo)"/> when reading them back.
    /// Number, date and time inputs always use the formats required by HTML.
    /// </summary>
    /// <param name="culture">The culture used to format values.</param>
    /// <returns>A new action using <paramref name="culture"/>.</returns>
    public UIModelUpdate WithCulture(CultureInfo culture)
    {
        return new UIModelUpdate(Name, _update, culture ?? throw new ArgumentNullException(nameof(culture)));
    }

    /// <inheritdoc />
    protected override void ExecuteWhen(IActor actor)
    {
        _update(actor, _culture);
    }
}
