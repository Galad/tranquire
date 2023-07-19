using System;
using FluentAssertions.Equivalency;

namespace Tranquire.Selenium.Tests;

internal static class EquivalencyOptionAssertionExtensions
{
    public static EquivalencyAssertionOptions<T> ExcludingDateTimeOffset<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding((IMemberInfo m) => m.Type == typeof(DateTimeOffset));
    }
}
