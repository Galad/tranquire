using System;
using OpenQA.Selenium;
using Tranquire.Selenium.Questions.UIModels;

namespace Tranquire.Selenium.Targets;

/// <summary>
/// An attribute that defines a target with parameters for Selenium actions.
/// When applied to a class, it specifies the container row target for use with
/// <c>UpdateUIModels.Update&lt;T&gt;(IEnumerable&lt;T&gt;)</c> where each element's
/// container is resolved by substituting <c>{PropertyName}</c> placeholders in the
/// format string with the corresponding property values of each model instance.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
public class TargetWithParametersAttribute : Attribute
{
    /// <summary>
    /// Gets the format string used to construct the target.
    /// </summary>
    public string Format { get; }

    /// <summary>
    /// Gets the format parameters used in the format string.
    /// </summary>
    public string[] FormatParameters { get; }

    /// <summary>
    /// Gets the Selenium <see cref="By"/> locator used to find the target.
    /// </summary>
    public By By { get; }

    /// <summary>
    /// Gets the method used to locate the target.
    /// </summary>
    public ByMethod ByMethod { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetWithParametersAttribute"/> class
    /// with a Selenium <see cref="By"/> locator, a format string, and optional format parameters.
    /// </summary>
    /// <param name="by">The Selenium <see cref="By"/> locator.</param>
    /// <param name="format">The format string used to construct the target.</param>
    /// <param name="formatParameters">The format parameters used in the format string.</param>
    public TargetWithParametersAttribute(By by, string format, params string[] formatParameters)
    {
        By = by;
        Format = format;
        FormatParameters = formatParameters ?? Array.Empty<string>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetWithParametersAttribute"/> class
    /// with a <see cref="ByMethod"/>, a format string, and optional format parameters.
    /// </summary>
    /// <param name="byMethod">The method used to locate the target.</param>
    /// <param name="format">The format string used to construct the target.</param>
    /// <param name="formatParameters">The format parameters used in the format string.</param>
    public TargetWithParametersAttribute(ByMethod byMethod, string format, params string[] formatParameters)
    {
        ByMethod = byMethod;
        Format = format;
        FormatParameters = formatParameters;
    }
}
