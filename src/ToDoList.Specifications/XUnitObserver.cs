using System;
using System.Diagnostics;
using System.Text;
using Tranquire.Reporting;
using Tranquire.Selenium;
using Xunit.Abstractions;

namespace ToDoList.Specifications
{
    public class XUnitObserver : IObserver<string>
    {
        private readonly Lazy<ITestOutputHelper> _helper;

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

    public class DebugObserver : IObserver<string>
    {
        public void OnCompleted()
        {
            Debug.WriteLine("Completed");
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine("Error " + error.Message);
        }

        public void OnNext(string value)
        {
            Debug.WriteLine(value);
        }
    }

    public class InMemoryObserver : IObserver<string>, IObserver<ScreenshotNotification>
    {
        private readonly StringBuilder _builder;

        public InMemoryObserver(StringBuilder builder)
        {
            _builder = builder;
        }

        public void OnCompleted()
        {
            _builder.AppendLine("On completed");
        }

        public void OnError(Exception error)
        {
            _builder.AppendLine("Error " + error.Message);
        }

        public void OnNext(string value)
        {
            _builder.AppendLine(value);
        }

        public void OnNext(ScreenshotNotification value)
        {
            _builder.AppendLine($"Screenshot! {value.FilePath}");
        }
    }
}