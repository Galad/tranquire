using System;
using Xunit.Abstractions;

namespace ToDoList.Specifications
{
    public class XUnitObserver : IObserver<string>
    {
        Lazy<ITestOutputHelper> _helper;

        public XUnitObserver(Func<ITestOutputHelper> helper)
        {
            _helper = new Lazy<ITestOutputHelper>(helper);
        }

        public void OnCompleted()
        {
            _helper.Value.WriteLine("Completed");
        }

        public void OnError(Exception error)
        {
            _helper.Value.WriteLine("Error " + error.Message);
        }

        public void OnNext(string value)
        {
            _helper.Value.WriteLine(value);
        }
    }
}