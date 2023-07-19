using System;

namespace Tranquire.Reporting;

/// <summary>
/// Measures the execution duration of a function
/// </summary>
public interface IMeasureDuration
{
    /// <summary>
    /// Measures the execution duration of a function
    /// </summary>
    /// <typeparam name="T">The type returned by the function</typeparam>
    /// <param name="function">The function to measure the duration of</param>
    /// <returns>A tuple containing the duration and the value returned by the function</returns>
    (TimeSpan, T, Exception) Measure<T>(Func<T> function);
    /// <summary>
    /// Gets the current date
    /// </summary>
    DateTimeOffset Now { get; }
}