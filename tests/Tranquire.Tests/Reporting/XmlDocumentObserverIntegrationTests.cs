using System;
using System.Globalization;
using System.Linq;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests.Reporting;

public class XmlDocumentObserverIntegrationTests
{
    private static readonly IAction<Unit> Action1 = Actions.Create("Action 1", a => a.Execute(Action2));
    private static readonly IAction<Unit> Action2 = Actions.Create("Action 2", a => a.Execute(Action3));
    private static readonly IAction<Unit> Action3 = Actions.Create("Action 3", a => { });
    private static readonly IAction<Unit> ActionWithQuestions = Actions.Create("Action with question", a => { a.AsksFor(Question1); });
    private static readonly IAction<Unit> ActionWithError = Actions.Create("Action with error", a => throw new InvalidOperationException("Error"));

    private static readonly IQuestion<string> Question1 = Questions.Create("Question 1", a => a.AsksFor(Question2));
    private static readonly IQuestion<string> Question2 = Questions.Create("Question 2", a =>
    {
        a.Execute(Action3);
        return a.AsksFor(Question3);
    });
    private static readonly IQuestion<string> Question3 = Questions.Create("Question 3", a => "answer");

    private readonly XmlDocumentObserver _xmlObserver;
    private readonly TestObserver<ActionNotification> _testObserver;
    private readonly Actor _actor;
    private readonly DateTimeOffset _now;
    private readonly TimeSpan _duration;

    private string StartDate => _now.ToString(CultureInfo.InvariantCulture);
    private string EndDate => _now.Add(_duration).ToString(CultureInfo.InvariantCulture);
    private int Duration => (int)_duration.TotalMilliseconds;

    private class MeasureDuration : IMeasureDuration
    {
        private readonly IMeasureDuration innerMeasureDuration;
        private readonly DateTimeOffset now;
        private readonly TimeSpan duration;

        public MeasureDuration(IMeasureDuration innerMeasureDuration, DateTimeOffset now, TimeSpan duration)
        {
            this.innerMeasureDuration = innerMeasureDuration;
            this.now = now;
            this.duration = duration;
        }

        public DateTimeOffset Now => now;

        public (TimeSpan, T, Exception) Measure<T>(Func<T> function)
        {
            var (t, r, e) = innerMeasureDuration.Measure(function);
            return (duration, r, e);
        }
    }

    public XmlDocumentObserverIntegrationTests()
    {
        _now = DateTimeOffset.UtcNow;
        _duration = TimeSpan.FromSeconds(5);
        var measureDuration = new MeasureDuration(new DefaultMeasureDuration(), _now, _duration);
        _xmlObserver = new XmlDocumentObserver(measureDuration);
        _testObserver = new TestObserver<ActionNotification>();
        _actor = new Actor("John").WithReporting(
            new CompositeObserver<ActionNotification>(
                _xmlObserver,
                _testObserver),
            measureDuration);
    }

    private T CreateItem<T>(string name, bool hasError = false) where T : XmlReportItem, new()
    {
        return new T() {
            StartDate = StartDate,
            EndDate = typeof(T) == typeof(XmlReportRoot) ? StartDate : EndDate, // because XmlReportRoot uses IMeasureDuration.Now to get the end date, instead of using IMeasureDuration.Measure
            Duration = typeof(T) == typeof(XmlReportRoot) ? 0 : Duration,
            Name = name,
            HasError = hasError
        };
    }

    #region Expected values        
    private XmlReportItem ExpectedAction1 =>
        CreateItem<XmlReportAction>(Action1.Name)
            .AddChildren(
                CreateItem<XmlReportAction>(Action2.Name)
                    .AddChildren(
                        CreateItem<XmlReportAction>(Action3.Name)
                    )
            );
    private XmlReportItem ExpectedActionWithQuestions =>
        CreateItem<XmlReportAction>(ActionWithQuestions.Name)
            .AddChildren(
                CreateItem<XmlReportQuestion>(Question1.Name)
                    .AddChildren(
                        CreateItem<XmlReportQuestion>(Question2.Name)
                            .AddChildren(
                                CreateItem<XmlReportAction>(Action3.Name),
                                CreateItem<XmlReportQuestion>(Question3.Name)
                            )
                    )
            );
    private XmlReportRoot MultipleStepExpected =>
        (XmlReportRoot)CreateItem<XmlReportRoot>("Test")
            .AddChildren(
                CreateItem<XmlReportGiven>("Given " + Action1.Name).AddChildren(ExpectedAction1),
                CreateItem<XmlReportGiven>("Given " + Action1.Name).AddChildren(ExpectedAction1),
                CreateItem<XmlReportWhen>("When " + ActionWithQuestions.Name).AddChildren(ExpectedActionWithQuestions)
                );
    private XmlReportRoot GivenWhenThenExpected =>
        (XmlReportRoot)CreateItem<XmlReportRoot>("Test")
            .AddChildren(
                CreateItem<XmlReportGiven>("Given " + Action3.Name)
                    .AddChildren(CreateItem<XmlReportAction>(Action3.Name)),
                CreateItem<XmlReportWhen>("When " + ActionWithQuestions.Name)
                    .AddChildren(ExpectedActionWithQuestions),
                CreateItem<XmlReportThen>("Then verifies the answer of " + Question3.Name)
                    .WithOutcome(XmlReportThenOutcome.pass)
                    .AddChildren(CreateItem<XmlReportQuestion>(Question3.Name))
            );
    #endregion

    [Fact]
    public void MultipleSteps()
    {
        // arrange
        _actor.Given(Action1, Action1);
        _actor.When(ActionWithQuestions);
        // act
        var actual = XmlDocumentObserverTests.DeserializeXmlDocument(_xmlObserver.GetXmlDocument());
        // arrange
        XmlDocumentObserverTests.AssertRootAreEqual(MultipleStepExpected, actual);
    }

    [Fact]
    public void GivenWhenThen()
    {
        // arrange
        _actor.Given(Action3);
        _actor.When(ActionWithQuestions);
        _actor.Then(Question3, _ => { });
        // act
        var actual = XmlDocumentObserverTests.DeserializeXmlDocument(_xmlObserver.GetXmlDocument());
        // arrange
        XmlDocumentObserverTests.AssertRootAreEqual(GivenWhenThenExpected, actual);
    }

    [Fact]
    public void Error()
    {
        // arrange
        try
        {
            _actor.When(ActionWithError);
        }
        catch (Exception) { }
        // act
        var actual = XmlDocumentObserverTests.DeserializeXmlDocument(_xmlObserver.GetXmlDocument());
        // arrange            
        var expectedError = _testObserver.Values.Select(v => v.Content)
                                                .OfType<ExecutionErrorNotificationContent>()
                                                .Select(n => n.Exception)
                                                .First();
        var expected = (XmlReportRoot)CreateItem<XmlReportRoot>("Test")
            .AddChildren(
                CreateItem<XmlReportWhen>("When " + ActionWithError.Name, true)
                    .AddChildren(
                        CreateItem<XmlReportAction>(ActionWithError.Name, true).WithError(expectedError.ToString())
                    )
            );
        XmlDocumentObserverTests.AssertRootAreEqual(expected, actual);
    }
}
