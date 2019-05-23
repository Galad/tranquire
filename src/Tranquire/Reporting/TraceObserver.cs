using System;
using System.Diagnostics;

namespace Tranquire.Reporting
{
    /// <summary>
    /// Writes notifications in the <see cref="System.Diagnostics.Trace" />
    /// </summary>
    public sealed class TraceObserver : IObserver<string>
    {
        /// <inheritdoc />
        public void OnCompleted()
        {
            Trace.WriteLine("Completed");
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            Trace.WriteLine("Error: " + error.Message);
            Trace.WriteLine(error.StackTrace);
        }

        /// <inheritdoc />
        public void OnNext(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Trace.WriteLine(value);
        }
    }
}

