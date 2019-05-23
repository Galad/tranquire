#if DEBUG && NET462
using System.Diagnostics;
using System.IO;
using System.Text;
using Tranquire.Reporting;

namespace Tranquire.Tests.Reporting
{
    public class DebugObserverTests : WriteStringObserverTestsBase<DebugObserver>
    {
        private readonly int _position;

        public DebugObserverTests() : this(new StringBuilder())
        {
        }

        private DebugObserverTests(StringBuilder @out) : base(@out)
        {
            _position = Debug.Listeners.Add(new TextWriterTraceListener(new StringWriter(@out)));
        }

        public override void Dispose()
        {
            Debug.Listeners.RemoveAt(_position);
        }
    }
}
#endif
