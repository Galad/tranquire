using System;

namespace Tranquire.Selenium.Actions.UIModels;

/// <summary>
/// Marks a property for Enter (text input) action when filling a form model.
/// Use this attribute alongside <see cref="Tranquire.Selenium.Questions.UIModels.TargetAttribute"/>
/// or <see cref="Tranquire.Selenium.Targets.TargetWithParametersAttribute"/> to specify
/// how the element is located and that its value should be entered via typing.
/// </summary>
/// <example>
/// <code>
/// public class Ingredient
/// {
///     [Target(ByMethod.Id, "ingredient-id"), Enter]
///     public string Id { get; set; }
///
///     [TargetWithParameters(ByMethod.Id, "row_{0}_name"), Enter]
///     public string Name { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class EnterAttribute : BaseFormModelAttribute
{
	/// <inheritdoc />
	public override void PerformFieldAction(
		IActor actor,
		string fieldKey,
		object value,
		string tagName,
		string type,
		ITarget fieldTarget,
		ITarget container)
	{
		UpdateField.PerformFieldAction(actor, fieldKey, value, tagName, type, fieldTarget, container);
	}
}
