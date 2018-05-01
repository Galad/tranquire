using System;
using System.Diagnostics;

namespace Tranquire.Reporting
{
    /// <summary>
    /// Writes notifications in the <see cref="System.Diagnostics.Debug">debugger</see>
    /// </summary>
    public class DebugObserver : IObserver<string>
    {
        /// <inheritdoc />
        public void OnCompleted()
        {
            Debug.WriteLine("Completed");
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            Debug.WriteLine("Error: " + error.Message);
            Debug.WriteLine(error.StackTrace);
        }

        /// <inheritdoc />
        public void OnNext(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Debug.WriteLine(value);
        }
    }
}
