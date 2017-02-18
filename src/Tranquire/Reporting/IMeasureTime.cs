using System;
using System.Diagnostics;

namespace Tranquire.Reporting
{
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
        Tuple<TimeSpan, T> Measure<T>(Func<T> function);
    }

    /// <summary>
    /// A default implementation of the duration measurement
    /// </summary>
    public class DefaultMeasureDuration : IMeasureDuration
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Tuple<TimeSpan, T> Measure<T>(Func<T> function)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            var sw = Stopwatch.StartNew();
            var value = function();
            sw.Stop();
            return Tuple.Create(sw.Elapsed, value);
        }
    }

}