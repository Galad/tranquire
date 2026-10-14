using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;

namespace Tranquire.Selenium.Actions.UIModels;

internal static class FormActions
{
    /// <summary>
    /// Fills all fields in the given container with the provided values.
    /// Supports parameterized keys in the values dictionary.
    /// </summary>
    /// <param name="container">The form or section container target.</param>
    /// <param name="values">Dictionary of parameterized field names and values to fill.</param>
    /// <param name="parameters">Parameters to resolve the field names.</param>
    /// <returns>An IAction that fills the form.</returns>
    public static IAction<Unit> FillFormFields(
        ITarget container,
        IReadOnlyDictionary<string, object> values,
        params object[] parameters)
    {
        return Tranquire.Actions.Create("Fill form fields in container", actor =>
        {
            // Pre-resolve all parameterized keys to actual field names
            var resolvedValues = values
                .Select(kv => new
                {
                    ResolvedKey = string.Format(kv.Key, parameters),
                    Value = kv.Value
                })
                .ToDictionary(x => x.ResolvedKey, x => x.Value);

            // Find all input, select, and textarea elements within the container
            var elements = actor.AsksFor(
                Element.Of(Target.The("All form fields")
                    .LocatedBy(By.CssSelector("input,select,textarea"))
                    .RelativeTo(container)
                ).Many()
            );

            foreach (var element in elements)
            {
                var name = element.GetAttribute("name");
                var id = element.GetAttribute("id");
                var key = !string.IsNullOrEmpty(name) && resolvedValues.ContainsKey(name)
                    ? name
                    : id;

                if (string.IsNullOrEmpty(key) || !resolvedValues.ContainsKey(key))
                {
                    continue; // Skip if no value provided for this field
                }

                var value = resolvedValues[key];
                var tagName = element.TagName.ToLowerInvariant();
                var type = (element.GetAttribute("type") ?? "").ToLowerInvariant();

                // Build a Target for this field, relative to the container
                ITarget fieldTarget;
                if (!string.IsNullOrEmpty(id))
                {
                    fieldTarget = Target.The($"{id} field")
                        .LocatedBy(By.Id(id))
                        .RelativeTo(container);
                }
                else
                {
                    fieldTarget = Target.The($"{name} field")
                        .LocatedBy(By.Name(name))
                        .RelativeTo(container);
                }

                UpdateField.PerformFieldAction(actor, key, value, tagName, type, fieldTarget, container);
            }
            return Unit.Default;
        });
    }

    public static IAction<Unit> FillFormFields(
        ITarget container,
        IReadOnlyDictionary<string, object> values)
    {
        return Tranquire.Actions.Create("Fill form fields in container", actor =>
        {
            var elements = actor.AsksFor(
                Element.Of(Target.The("All form fields")
                    .LocatedBy(By.CssSelector("input,select,textarea"))
                    .RelativeTo(container)
                ).Many()
            );

            foreach (var element in elements)
            {
                var name = element.GetAttribute("name");
                var id = element.GetAttribute("id");
                var key = !string.IsNullOrEmpty(name) && values.ContainsKey(name)
                    ? name
                    : id;

                if (string.IsNullOrEmpty(key) || !values.ContainsKey(key))
                {
                    continue; // Skip if no value provided for this field
                }

                var value = values[key];
                var tagName = element.TagName.ToLowerInvariant();
                var type = (element.GetAttribute("type") ?? "").ToLowerInvariant();

                ITarget fieldTarget;
                if (!string.IsNullOrEmpty(id))
                {
                    fieldTarget = Target.The($"{id} field")
                        .LocatedBy(By.Id(id))
                        .RelativeTo(container);
                }
                else
                {
                    fieldTarget = Target.The($"{name} field")
                        .LocatedBy(By.Name(name))
                        .RelativeTo(container);
                }

                UpdateField.PerformFieldAction(actor, key, value, tagName, type, fieldTarget, container);
            }
            return Unit.Default;
        });
    }
}
