using System;
using System.Collections;
using System.Collections.Generic;

namespace Tranquire;

/// <summary>
/// A composite implementation of <see cref="IObserver{T}"/> 
/// </summary>
/// <typeparam name="T"></typeparam>
public class CompositeObserver<T> : IEnumerable<IObserver<T>>, IObserver<T>
{
    private readonly IEnumerable<IObserver<T>> _observers;

    /// <summary>
    /// Creates a new instance of <see cref="CompositeObserver{T}"/>
    /// </summary>
    /// <param name="observers"></param>
    public CompositeObserver(params IObserver<T>[] observers)
    {
        _observers = observers ?? throw new ArgumentNullException(nameof(observers));
    }

    /// <inheritdoc />
    public void OnCompleted()
    {
        foreach (var observer in _observers)
        {
            observer.OnCompleted();
        }
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        foreach (var observer in _observers)
        {
            observer.OnError(error);
        }
    }

    /// <inheritdoc />
    public void OnNext(T value)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _observers.GetEnumerator();
    }

    /// <inheritdoc />
    public IEnumerator<IObserver<T>> GetEnumerator()
    {
        return _observers.GetEnumerator();
    }
}
