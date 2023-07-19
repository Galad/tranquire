using System;

namespace Tranquire.Reporting;

/// <summary>
/// Writes notifications in the <see cref="System.Console">console</see>
/// </summary>
public class ConsoleObserver : IObserver<string>
{
    /// <inheritdoc />
    public void OnCompleted()
    {
        Console.WriteLine("Completed");
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        if (error == null)
        {
            throw new ArgumentNullException(nameof(error));
        }

        Console.WriteLine("Error: " + error.Message);
        Console.WriteLine(error.StackTrace);
    }

    /// <inheritdoc />
    public void OnNext(string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Console.WriteLine(value);
    }
}

