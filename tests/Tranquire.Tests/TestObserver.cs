using System;
using System.Collections.Generic;

namespace Tranquire.Tests
{
    public class TestObserver<T> : IObserver<T>
    {
        public TestObserver()
        {
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        private readonly List<T> _values = new List<T>();

        public IReadOnlyList<T> Values => _values;
        public void OnNext(T value)
        {
            _values.Add(value);
        }
    }
}
