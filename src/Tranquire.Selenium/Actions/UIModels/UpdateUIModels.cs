using System;
using System.Collections.Generic;

namespace Tranquire.Selenium.Actions.UIModels;

/// <summary>
/// Provides methods to update UI models.
/// </summary>
public static class UpdateUIModels
{
    /// <summary>
    /// Updates the UI models with the provided values from a model.
    /// </summary>
    /// <typeparam name="T">The type of the model containing the values to update the UI models.</typeparam>
    /// <param name="value">The model containing the values to update the UI models.</param>
    /// <param name="container">The container target for the UI models.</param>
    /// <param name="propertiesTargetParameters">Optional parameters to resolve field names.</param>
    /// <returns>An IAction that updates the UI models.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is a collection.</exception>
    public static UIModelUpdate Update<T>(
        T value,
        ITarget container,
        params string[] propertiesTargetParameters)
    {
        return FormUpdate.FillFormFromModel(value, container, propertiesTargetParameters);
    }

    /// <summary>
    /// Updates the UI models with the provided values from a model.
    /// </summary>
    /// <typeparam name="T">The type of the model containing the values to update the UI models.</typeparam>
    /// <param name="value">The model containing the values to update the UI models.</param>
    /// <param name="container">The container target for the UI models.</param>
    /// <returns>An IAction that updates the UI models.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is a collection.</exception>
    public static UIModelUpdate Update<T>(
        T value,
        ITarget container)
    {
        return FormUpdate.FillFormFromModel(value, container);
    }

    /// <summary>
    /// Updates the UI for each item in <paramref name="values"/> using the class-level
    /// <see cref="Tranquire.Selenium.Targets.TargetWithParametersAttribute"/> on <typeparamref name="T"/>
    /// to resolve each item's container target via <c>{PropertyName}</c> placeholder substitution.
    /// </summary>
    /// <typeparam name="T">
    /// The model type. Must be decorated with
    /// <c>[TargetWithParameters(ByMethod, "format_{PropertyName}")]</c> at the class level.
    /// </typeparam>
    /// <param name="values">The collection of model instances to fill into the form.</param>
    /// <returns>An IAction that fills the form for each item.</returns>
    /// <remarks>Null items are skipped.</remarks>
    /// <example>
    /// <code>
    /// [TargetWithParameters(ByMethod.Id, "row_{Id}")]
    /// public class Ingredient { public int Id { get; set; } public string Name { get; set; } }
    ///
    /// var action = UpdateUIModels.Update(ingredients);
    /// actor.When(action);
    /// </code>
    /// </example>
    public static UIModelUpdate Update<T>(IEnumerable<T> values)
    {
        return FormUpdate.FillFormFromModels(values);
    }

    /// <summary>
    /// Updates the UI for each item in <paramref name="values"/>, using
    /// <paramref name="targetSelector"/> to derive each row's container from the
    /// parameterizable <paramref name="container"/>, and
    /// <paramref name="propertiesTargetParameters"/> to resolve property-level parameterized targets.
    /// </summary>
    /// <typeparam name="T">The model type.</typeparam>
    /// <param name="values">The collection of model instances.</param>
    /// <param name="container">A parameterizable container target (e.g. <c>Target.The("row").LocatedBy("row_{0}", By.Id)</c>).</param>
    /// <param name="targetSelector">Maps each item and the container to the concrete row target.</param>
    /// <param name="propertiesTargetParameters">
    /// Optional function returning the parameter strings used to resolve property-level
    /// <see cref="Tranquire.Selenium.Targets.TargetWithParametersAttribute"/> selectors for each item.
    /// </param>
    /// <returns>An IAction that fills the form for each item.</returns>
    /// <remarks>
    /// Null items are skipped. Omit <paramref name="propertiesTargetParameters"/> rather than passing <c>null</c>,
    /// which is ambiguous with the <c>params string[]</c> overload.
    /// </remarks>
    /// <example>
    /// <code>
    /// var rowTarget = Target.The("row").LocatedBy("row_{0}", By.Id);
    /// var action = UpdateUIModels.Update(
    ///     ingredients,
    ///     rowTarget,
    ///     (ing, container) => container.Of(ing.Id.ToString()));
    /// actor.When(action);
    /// </code>
    /// </example>
    public static UIModelUpdate Update<T>(
        IEnumerable<T> values,
        ITargetWithParameters container,
        Func<T, ITargetWithParameters, ITarget> targetSelector,
        Func<T, string[]> propertiesTargetParameters = null)
    {
        return FormUpdate.FillFormFromModels(values, container, targetSelector, propertiesTargetParameters);
    }

    /// <summary>
    /// Updates the UI for each item in <paramref name="values"/>, using
    /// <paramref name="targetSelector"/> to derive each row's container from the
    /// parameterizable <paramref name="container"/>, passing shared
    /// <paramref name="propertiesTargetParameters"/> to all property-level parameterized targets.
    /// </summary>
    /// <typeparam name="T">The model type.</typeparam>
    /// <param name="values">The collection of model instances.</param>
    /// <param name="container">A parameterizable container target.</param>
    /// <param name="targetSelector">Maps each item and the container to the concrete row target.</param>
    /// <param name="propertiesTargetParameters">
    /// Shared parameters used to resolve property-level
    /// <see cref="Tranquire.Selenium.Targets.TargetWithParametersAttribute"/> selectors.
    /// </param>
    /// <returns>An IAction that fills the form for each item.</returns>
    /// <remarks>Null items are skipped.</remarks>
    public static UIModelUpdate Update<T>(
        IEnumerable<T> values,
        ITargetWithParameters container,
        Func<T, ITargetWithParameters, ITarget> targetSelector,
        params string[] propertiesTargetParameters)
    {
        return FormUpdate.FillFormFromModels(values, container, targetSelector, propertiesTargetParameters);
    }
}
