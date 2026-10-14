using System;

namespace Tranquire.Selenium.Actions.UIModels;

/// <summary>
/// Base class for attributes that define how a property should be interacted with
/// when filling a form model.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public abstract class BaseFormModelAttribute : Attribute
{
	/// <summary>
	/// Performs this attribute's form update for the resolved field.
	/// Derived attributes can provide custom interactions while reusing target resolution.
	/// </summary>
	public abstract void PerformFieldAction(
		IActor actor,
		string fieldKey,
		object value,
		string tagName,
		string type,
		ITarget fieldTarget,
		ITarget container);
}
