using System;

namespace Tranquire.Reporting;

internal interface INotificationContentFactory
{
    IActionNotificationContent CreateBefore<T>(INamed action, DateTimeOffset date, CommandType commandType);
    IActionNotificationContent CreateAfter(TimeSpan time);
    IActionNotificationContent CreateError(Exception error, TimeSpan time);
}

internal class ThenNotificationContentFactory : INotificationContentFactory
{
    public IActionNotificationContent CreateAfter(TimeSpan time)
    {
        return new AfterThenNotificationContent(time, ThenOutcome.Pass);
    }

    public IActionNotificationContent CreateBefore<T>(INamed action, DateTimeOffset date, CommandType commandType)
    {
        return new BeforeThenNotificationContent(date, (action as ThenAction<T>).Question);
    }

    public IActionNotificationContent CreateError(Exception error, TimeSpan time)
    {
        return new AfterThenNotificationContent(time, GetOutcome(error), error);
    }

    private static ThenOutcome GetOutcome(Exception error)
    {
        if (Array.Exists(_knownNamespaces, error.GetType().FullName.StartsWith))
        {
            return ThenOutcome.Failed;
        }
        return ThenOutcome.Error;
    }

    private static readonly string[] _knownNamespaces = new[]
    {
        "Xunit.Sdk.",
        "NUnit.Framework.",
        "Microsoft.VisualStudio.TestTools.UnitTesting."
    };
}

internal class CommandNotificationContentfactory : INotificationContentFactory
{
    public IActionNotificationContent CreateAfter(TimeSpan time)
    {
        return new AfterActionNotificationContent(time);
    }

    public IActionNotificationContent CreateBefore<T>(INamed action, DateTimeOffset date, CommandType commandType)
    {
        return new BeforeFirstActionNotificationContent(date, (action as CommandAction<T>).ActionContext);
    }

    public IActionNotificationContent CreateError(Exception error, TimeSpan time)
    {
        return new ExecutionErrorNotificationContent(error, time);
    }
}

internal class DefaultNotificationContentFactory : INotificationContentFactory
{
    public IActionNotificationContent CreateAfter(TimeSpan time)
    {
        return new AfterActionNotificationContent(time);
    }

    public IActionNotificationContent CreateBefore<T>(INamed action, DateTimeOffset date, CommandType commandType)
    {
        return new BeforeActionNotificationContent(date, commandType);
    }

    public IActionNotificationContent CreateError(Exception error, TimeSpan time)
    {
        return new ExecutionErrorNotificationContent(error, time);
    }
}
