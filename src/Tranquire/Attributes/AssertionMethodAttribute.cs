using System;

namespace Tranquire.Attributes;

/// <summary>
/// Flags a method as an assertion method.
/// </summary>
/// <remarks>
/// This attribute is useful for static analysis tools.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class AssertionMethodAttribute : Attribute
{
}
