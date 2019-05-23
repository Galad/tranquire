using System.Diagnostics;
using System.IO;
using System.Text;
using Tranquire.Reporting;

namespace Tranquire.Tests.Reporting
{
    public class TraceObserverTests : WriteStringObserverTestsBase<TraceObserver>
    {
        private readonly int _position;

        public TraceObserverTests() : this(new StringBuilder()) { }

        private TraceObserverTests(StringBuilder @out) : base(@out)
        {
            _position = Trace.Listeners.Add(new TextWriterTraceListener(new StringWriter(@out)));
        }

        public override void Dispose()
        {
            Trace.Listeners.RemoveAt(_position);
            base.Dispose();
        }
    }
}

