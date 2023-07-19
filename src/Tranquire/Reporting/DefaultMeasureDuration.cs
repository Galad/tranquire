using System;
using System.Diagnostics;

namespace Tranquire.Reporting;

/// <summary>
/// A default implementation of the duration measurement
/// </summary>
public class DefaultMeasureDuration : IMeasureDuration
{
    /// <inheritdoc />
    public DateTimeOffset Now => DateTimeOffset.UtcNow;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public (TimeSpan, T, Exception) Measure<T>(Func<T> function)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        if (function == null)
        {
            throw new ArgumentNullException(nameof(function));
        }

        var sw = Stopwatch.StartNew();
        try
        {
            var value = function();
            sw.Stop();
            return (sw.Elapsed, value, null);
        }
        catch (Exception ex)
        {
            sw.Stop();
            return (sw.Elapsed, default, ex);
        }
    }
}