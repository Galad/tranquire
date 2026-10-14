using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;
using Tranquire.Selenium.Questions;
using Tranquire.Selenium.Questions.UIModels;
using Tranquire.Selenium.Targets;

namespace Tranquire.Selenium.Actions.UIModels;

/// <summary>
/// Provides methods to update form elements in a UI model.
/// </summary>
internal static class FormUpdate
{
    public static IAction<Unit> FillFormFromModel<T>(
        T value,
        ITarget container,
        params string[] propertiesTargetParameters)
    {
        return FillFormFromModelCore(value, container, propertiesTargetParameters);
    }
    public static IAction<Unit> FillFormFromModel<T>(
        T value,
        ITarget container)
    {
        return FillFormFromModelCore(value, container, Array.Empty<string>());
    }

    private static IAction<Unit> FillFormFromModelCore<T>(
        T value,
        ITarget container,
        string[] propertiesTargetParameters)
    {
        return Tranquire.Actions.Create(
            $"Fill form for {typeof(T).Name}",
            actor =>
            {
                if (value != null)
                    UpdateModelFields(actor, value, container, propertiesTargetParameters ?? Array.Empty<string>());
                return Unit.Default;
            });
    }

    private static void UpdateModelFields(
        IActor actor,
        object model,
        ITarget container,
        string[] propertiesTargetParameters)
    {
        foreach (var property in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            UpdateModelProperty(actor, model, property, container, propertiesTargetParameters);
    }

    private static void UpdateModelProperty(
        IActor actor,
        object model,
        PropertyInfo property,
        ITarget container,
        string[] propertiesTargetParameters)
    {
        var value = property.GetValue(model);
        if (value == null || TryPerformFieldUpdate(actor, model, property, value, container, propertiesTargetParameters))
            return;

        if (value is IEnumerable nestedValues && value is not string)
        {
            UpdateNestedCollection(actor, model, property, nestedValues, container, propertiesTargetParameters);
            return;
        }

        if (HasTargetMetadata(value.GetType()))
        {
            var nestedContainer = ResolveNestedContainer(property, model, value, container);
            UpdateModelFields(actor, value, nestedContainer, propertiesTargetParameters);
        }
    }

    private static void UpdateNestedCollection(
        IActor actor,
        object parentModel,
        PropertyInfo property,
        IEnumerable nestedValues,
        ITarget container,
        string[] propertiesTargetParameters)
    {
        var nestedContainer = ResolveNestedContainer(property, parentModel, nestedValues, container);
        foreach (var nestedValue in nestedValues)
        {
            if (nestedValue == null || !HasTargetMetadata(nestedValue.GetType()))
                continue;

            var itemContainer = ResolveNestedContainer(null, parentModel, nestedValue, nestedContainer);
            UpdateModelFields(actor, nestedValue, itemContainer, propertiesTargetParameters);
        }
    }

    private static bool TryPerformFieldUpdate(
        IActor actor,
        object model,
        PropertyInfo property,
        object value,
        ITarget container,
        string[] propertiesTargetParameters)
    {
        var formModelAttributes = property.GetCustomAttributes<BaseFormModelAttribute>().ToArray();
        var targetWithParamsAttr = property.GetCustomAttribute<TargetWithParametersAttribute>();
        ITarget fieldTarget;

        if (targetWithParamsAttr != null)
        {
            fieldTarget = ResolveTarget.ResolveTargets(
                property,
                model,
                value,
                propertiesTargetParameters,
                targetWithParamsAttr,
                container);
        }
        else
        {
            var targetAttr = property.GetCustomAttribute<TargetAttribute>();
            if (targetAttr == null || formModelAttributes.Length == 0)
                return false;

            fieldTarget = ResolveAttributeTarget(targetAttr, property.Name, model, container);
        }

        PerformFieldActions(actor, property.Name, value, fieldTarget, container, formModelAttributes);
        return true;
    }

    private static void PerformFieldActions(
        IActor actor,
        string fieldKey,
        object value,
        ITarget fieldTarget,
        ITarget container,
        BaseFormModelAttribute[] formModelAttributes)
    {
        var type = actor.AsksFor(HtmlAttribute.Of(fieldTarget).Named("type"))?.ToLowerInvariant();
        var tagName = actor.AsksFor(HtmlAttribute.Of(fieldTarget).Named("tagName"))?.ToLowerInvariant() ?? "input";

        if (formModelAttributes.Length == 0)
        {
            UpdateField.PerformFieldAction(actor, fieldKey, value, tagName, type, fieldTarget, container);
            return;
        }

        foreach (var attribute in formModelAttributes)
        {
            attribute.PerformFieldAction(actor, fieldKey, value, tagName, type, fieldTarget, container);
        }
    }

    private static ITarget ResolveNestedContainer(
        PropertyInfo property,
        object parentModel,
        object nestedModel,
        ITarget container)
    {
        var nestedType = nestedModel.GetType();
        var classTargetWithParameters = nestedType.GetCustomAttribute<TargetWithParametersAttribute>();
        if (classTargetWithParameters != null)
        {
            var selector = ResolveTarget.ResolveClassLevelFormat(classTargetWithParameters, nestedModel);
            var by = ResolveTarget.ResolveBy(classTargetWithParameters, selector);
            return Target.The(nestedType.Name).LocatedBy(by).RelativeTo(container);
        }

        var targetAttribute = property?.GetCustomAttribute<TargetAttribute>() ??
            nestedType.GetCustomAttribute<TargetAttribute>();
        if (targetAttribute == null || targetAttribute.ByMethod == ByMethod.Self)
            return container;

        var targetModel = property == null ? nestedModel : parentModel;
        return ResolveAttributeTarget(
            targetAttribute,
            property?.Name ?? nestedType.Name,
            targetModel,
            container);
    }

    private static ITarget ResolveAttributeTarget(
        TargetAttribute targetAttribute,
        string targetName,
        object model,
        ITarget container)
    {
        if (targetAttribute.ByMethod == ByMethod.Self)
            return container;

        var selector = ResolveTarget.ResolveNamedFormat(targetAttribute.Value, model);
        var name = string.IsNullOrEmpty(targetAttribute.Name) ? targetName : targetAttribute.Name;
        return Target.The(name)
            .LocatedBy(ResolveTarget.ResolveBy(targetAttribute.ByMethod, selector))
            .RelativeTo(container);
    }

    private static bool HasTargetMetadata(Type modelType)
    {
        if (modelType.GetCustomAttribute<TargetAttribute>() != null ||
            modelType.GetCustomAttribute<TargetWithParametersAttribute>() != null)
            return true;

        foreach (var property in modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (property.GetCustomAttribute<TargetAttribute>() != null ||
                property.GetCustomAttribute<TargetWithParametersAttribute>() != null)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Fills the form for each item in <paramref name="values"/> using the class-level
    /// <see cref="TargetWithParametersAttribute"/> on <typeparamref name="T"/> to resolve
    /// the container target per item (supports <c>{PropertyName}</c> placeholders).
    /// </summary>
    public static IAction<Unit> FillFormFromModels<T>(IEnumerable<T> values)
    {
        return Tranquire.Actions.Create(
            $"Fill form for collection of {typeof(T).Name}",
            actor =>
            {
                var classAttr = typeof(T).GetCustomAttribute<TargetWithParametersAttribute>();
                if (classAttr == null)
                {
                    throw new InvalidOperationException(
                        $"Type '{typeof(T).Name}' must have a [TargetWithParameters] attribute at the class level " +
                        $"when using UpdateUIModels.Update(IEnumerable<T>).");
                }

                foreach (var value in values)
                {
                    var selector = ResolveTarget.ResolveClassLevelFormat(classAttr, value);
                    var by = ResolveTarget.ResolveBy(classAttr, selector);
                    var container = Target.The(typeof(T).Name).LocatedBy(by);
                    actor.Execute(FillFormFromModel(value, container));
                }
                return Unit.Default;
            });
    }

    /// <summary>
    /// Fills the form for each item in <paramref name="values"/>, using <paramref name="targetSelector"/>
    /// to derive the container target for each item from the parameterizable <paramref name="container"/>.
    /// </summary>
    public static IAction<Unit> FillFormFromModels<T>(
        IEnumerable<T> values,
        ITargetWithParameters container,
        Func<T, ITargetWithParameters, ITarget> targetSelector,
        Func<T, string[]> propertiesTargetParameters = null)
    {
        return Tranquire.Actions.Create(
            $"Fill form for collection of {typeof(T).Name}",
            actor =>
            {
                foreach (var value in values)
                {
                    var rowContainer = targetSelector(value, container);
                    var parameters = propertiesTargetParameters?.Invoke(value) ?? Array.Empty<string>();
                    actor.Execute(FillFormFromModel(value, rowContainer, parameters));
                }
                return Unit.Default;
            });
    }

    /// <summary>
    /// Fills the form for each item in <paramref name="values"/>, using <paramref name="targetSelector"/>
    /// to derive the container target for each item from the parameterizable <paramref name="container"/>,
    /// passing <paramref name="propertiesTargetParameters"/> to resolve property-level parameterized targets.
    /// </summary>
    public static IAction<Unit> FillFormFromModels<T>(
        IEnumerable<T> values,
        ITargetWithParameters container,
        Func<T, ITargetWithParameters, ITarget> targetSelector,
        params string[] propertiesTargetParameters)
    {
        return Tranquire.Actions.Create(
            $"Fill form for collection of {typeof(T).Name}",
            actor =>
            {
                foreach (var value in values)
                {
                    var rowContainer = targetSelector(value, container);
                    actor.Execute(FillFormFromModel(value, rowContainer, propertiesTargetParameters));
                }
                return Unit.Default;
            });
    }
}
