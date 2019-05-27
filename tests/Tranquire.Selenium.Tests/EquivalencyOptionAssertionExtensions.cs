using FluentAssertions.Equivalency;
using System;

namespace Tranquire.Selenium.Tests
{
    internal static class EquivalencyOptionAssertionExtensions
    {
        public static EquivalencyAssertionOptions<T> ExcludingDateTimeOffset<T>(this EquivalencyAssertionOptions<T> options)
        {
            return options.Excluding((IMemberInfo m) => m.RuntimeType == typeof(DateTimeOffset));
        }
    }
}
