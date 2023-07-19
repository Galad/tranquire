using System;
using System.IO;
using System.Text;
using Tranquire.Reporting;

namespace Tranquire.Tests.Reporting;

public class ConsoleObserverTests : WriteStringObserverTestsBase<ConsoleObserver>
{
    private readonly TextWriter _standardOut;

    public ConsoleObserverTests() : this(new StringBuilder())
    {

    }

    private ConsoleObserverTests(StringBuilder @out) : base(@out)
    {
        _standardOut = Console.Out;
        Console.SetOut(new StringWriter(@out));
    }

    public override void Dispose()
    {
        Console.SetOut(_standardOut);
    }
}
