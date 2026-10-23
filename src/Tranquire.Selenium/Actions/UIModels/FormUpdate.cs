using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Tranquire.Selenium.Questions;
using Tranquire.Selenium.Questions.UIModels;
using Tranquire.Selenium.Targets;

namespace Tranquire.Selenium.Actions.UIModels;

/// <summary>
/// Provides methods to update form elements in a UI model.
/// </summary>
internal static class FormUpdate
{
    public static UIModelUpdate FillFormFromModel<T>(
        T value,
        ITarget container,
        params string[] propertiesTargetParameters)
    {
        return FillFormFromModelCore(value, container, propertiesTargetParameters);
    }
    public static UIModelUpdate FillFormFromModel<T>(
        T value,
        ITarget container)
    {
        return FillFormFromModelCore(value, container, Array.Empty<string>());
    }

    private static UIModelUpdate FillFormFromModelCore<T>(
        T value,
        ITarget container,
        string[] propertiesTargetParameters)
    {
        if (value is IEnumerable)
            throw new ArgumentException(
                $"'{typeof(T).Name}' cannot be updated as a single model. Use an UpdateUIModels.Update overload that takes an IEnumerable<T>.",
                nameof(value));

        return new UIModelUpdate(
            $"Fill form for {typeof(T).Name}",
            (actor, culture) =>
            {
                if (value is not null)
                {
                    var context = new UpdateContext(actor, propertiesTargetParameters ?? Array.Empty<string>(), culture);
                    UpdateModelFields(context, value, container);
                }
            });
    }

    private sealed class UpdateContext
    {
        public UpdateContext(IActor actor, string[] propertiesTargetParameters, CultureInfo culture)
        {
            Actor = actor;
            PropertiesTargetParameters = propertiesTargetParameters;
            Culture = culture;
        }

        public IActor Actor { get; }
        public string[] PropertiesTargetParameters { get; }
        public CultureInfo Culture { get; }
        public Stack<object> Ancestors { get; } = new();
    }

    private static void UpdateModelFields(UpdateContext context, object model, ITarget container)
    {
        // A back-reference to a model being updated would otherwise recurse forever.
        if (context.Ancestors.Any(ancestor => ReferenceEquals(ancestor, model)))
            return;

        context.Ancestors.Push(model);
        foreach (var property in ModelProperties.Of(model.GetType()))
            UpdateModelProperty(context, model, property, container);
        context.Ancestors.Pop();
    }

    private static void UpdateModelProperty(
        UpdateContext context,
        object model,
        PropertyInfo property,
        ITarget container)
    {
        var value = property.GetValue(model);
        if (value == null ||
            IsDefaultImmutableArray(value) ||
            TryPerformFieldUpdate(context, model, property, value, container))
            return;

        if (value is IEnumerable nestedValues && value is not string)
        {
            UpdateNestedCollection(context, model, property, nestedValues, container);
            return;
        }

        if (HasTargetMetadata(value.GetType()))
        {
            var nestedContainer = ResolveNestedContainer(property, model, value, container);
            UpdateModelFields(context, value, nestedContainer);
        }
    }

    private static bool IsDefaultImmutableArray(object value)
    {
        var type = value.GetType();
        return type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(ImmutableArray<>) &&
            (bool)type.GetProperty(nameof(ImmutableArray<object>.IsDefault)).GetValue(value);
    }

    private static void UpdateNestedCollection(
        UpdateContext context,
        object parentModel,
        PropertyInfo property,
        IEnumerable nestedValues,
        ITarget container)
    {
        var nestedContainer = ResolveNestedContainer(property, parentModel, nestedValues, container);
        foreach (var nestedValue in nestedValues)
        {
            if (nestedValue == null || !HasTargetMetadata(nestedValue.GetType()))
                continue;

            var itemContainer = ResolveNestedContainer(null, parentModel, nestedValue, nestedContainer);
            UpdateModelFields(context, nestedValue, itemContainer);
        }
    }

    private static bool TryPerformFieldUpdate(
        UpdateContext context,
        object model,
        PropertyInfo property,
        object value,
        ITarget container)
    {
        var formModelAttributes = property.GetCustomAttributes<BaseFormModelAttribute>().ToArray();
        if (formModelAttributes.Length == 0)
            return false;

        var targetWithParamsAttr = property.GetCustomAttribute<TargetWithParametersAttribute>();
        ITarget fieldTarget;

        if (targetWithParamsAttr != null)
        {
            fieldTarget = ResolveTarget.ResolveTargets(
                property,
                model,
                value,
                context.PropertiesTargetParameters,
                targetWithParamsAttr,
                container);
        }
        else
        {
            var targetAttr = property.GetCustomAttribute<TargetAttribute>();
            if (targetAttr == null)
                return false;

            fieldTarget = ResolveAttributeTarget(
                targetAttr,
                property.Name,
                model,
                container,
                value,
                context.PropertiesTargetParameters);
        }

        var actor = context.Actor;
        var type = actor.AsksFor(HtmlAttribute.Of(fieldTarget).Named("type"))?.ToLowerInvariant();
        var tagName = actor.AsksFor(HtmlAttribute.Of(fieldTarget).Named("tagName"))?.ToLowerInvariant() ?? "input";
        var field = new FormField(property.Name, value, tagName, type, fieldTarget, container, context.Culture);
        foreach (var attribute in formModelAttributes)
        {
            attribute.PerformFieldAction(actor, field);
        }
        return true;
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
            return ResolveTarget.ResolveClassTarget(classTargetWithParameters, nestedModel, nestedType.Name).RelativeTo(container);

        var propertyTargetAttribute = property?.GetCustomAttribute<TargetAttribute>();
        var targetAttribute = propertyTargetAttribute ?? nestedType.GetCustomAttribute<TargetAttribute>();
        if (targetAttribute == null)
            return container;

        var targetModel = propertyTargetAttribute == null ? nestedModel : parentModel;
        return ResolveAttributeTarget(
            targetAttribute,
            property?.Name ?? nestedType.Name,
            targetModel,
            container,
            null,
            Array.Empty<string>());
    }

    private static ITarget ResolveAttributeTarget(
        TargetAttribute targetAttribute,
        string targetName,
        object model,
        ITarget container,
        object currentValue,
        string[] propertiesTargetParameters)
    {
        var name = targetAttribute.Name ?? targetName;
        // Same target as UIModel builds when reading; only templated selectors need resolving.
        if (!ResolveTarget.HasPlaceholders(targetAttribute.Value))
            return targetAttribute.CreateTarget(name).RelativeTo(container);

        var selector = ResolveTarget.ResolveNamedFormat(
            targetAttribute.Value,
            model,
            targetAttribute.ByMethod,
            currentValue,
            propertiesTargetParameters);
        return Target.The(name)
            .LocatedBy(targetAttribute.ByMethod.ToBy(selector))
            .RelativeTo(container);
    }

    private static bool HasTargetMetadata(Type modelType)
    {
        if (modelType.GetCustomAttribute<TargetAttribute>() != null ||
            modelType.GetCustomAttribute<TargetWithParametersAttribute>() != null)
            return true;

        return Array.Exists(
            ModelProperties.Of(modelType),
            property => property.GetCustomAttribute<TargetAttribute>() != null ||
                property.GetCustomAttribute<TargetWithParametersAttribute>() != null);
    }

    /// <summary>
    /// Fills the form for each item in <paramref name="values"/> using the class-level
    /// <see cref="TargetWithParametersAttribute"/> on <typeparamref name="T"/> to resolve
    /// the container target per item (supports <c>{PropertyName}</c> placeholders).
    /// </summary>
    public static UIModelUpdate FillFormFromModels<T>(IEnumerable<T> values)
    {
        return new UIModelUpdate(
            $"Fill form for collection of {typeof(T).Name}",
            (actor, culture) =>
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
                    if (value is null)
                        continue;

                    var container = ResolveTarget.ResolveClassTarget(classAttr, value, typeof(T).Name);
                    actor.Execute(FillFormFromModel(value, container).WithCulture(culture));
                }
            });
    }

    /// <summary>
    /// Fills the form for each item in <paramref name="values"/>, using <paramref name="targetSelector"/>
    /// to derive the container target for each item from the parameterizable <paramref name="container"/>.
    /// </summary>
    public static UIModelUpdate FillFormFromModels<T>(
        IEnumerable<T> values,
        ITargetWithParameters container,
        Func<T, ITargetWithParameters, ITarget> targetSelector,
        Func<T, string[]> propertiesTargetParameters = null)
    {
        return new UIModelUpdate(
            $"Fill form for collection of {typeof(T).Name}",
            (actor, culture) =>
            {
                foreach (var value in values)
                {
                    if (value is null)
                        continue;

                    var rowContainer = targetSelector(value, container);
                    var parameters = propertiesTargetParameters?.Invoke(value) ?? Array.Empty<string>();
                    actor.Execute(FillFormFromModel(value, rowContainer, parameters).WithCulture(culture));
                }
            });
    }

    /// <summary>
    /// Fills the form for each item in <paramref name="values"/>, using <paramref name="targetSelector"/>
    /// to derive the container target for each item from the parameterizable <paramref name="container"/>,
    /// passing <paramref name="propertiesTargetParameters"/> to resolve property-level parameterized targets.
    /// </summary>
    public static UIModelUpdate FillFormFromModels<T>(
        IEnumerable<T> values,
        ITargetWithParameters container,
        Func<T, ITargetWithParameters, ITarget> targetSelector,
        params string[] propertiesTargetParameters)
    {
        return FillFormFromModels(values, container, targetSelector, value => propertiesTargetParameters);
    }
}
