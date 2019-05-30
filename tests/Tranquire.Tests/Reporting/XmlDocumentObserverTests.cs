using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests.Reporting
{
    public abstract class XmlReportItem
    {
        [XmlAttribute("start-date")]
        public string StartDate { get; set; }
        [XmlAttribute("end-date")]
        public string EndDate { get; set; }
        [XmlAttribute("duration")]
        public int Duration { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("has-error")]
        public bool HasError { get; set; }
        [XmlElement(typeof(XmlReportAction), ElementName = "action")]
        [XmlElement(typeof(XmlReportQuestion), ElementName = "question")]
        [XmlElement(typeof(XmlReportGiven), ElementName = "given")]
        [XmlElement(typeof(XmlReportWhen), ElementName = "when")]
        [XmlElement(typeof(XmlReportThen), ElementName = "then")]
        public List<XmlReportItem> Children { get; set; } = new List<XmlReportItem>();
        [XmlArray("attachments")]
        [XmlArrayItem("attachment")]
        public List<XlmActionAttachment> Attachments { get; set; } = new List<XlmActionAttachment>();
        [XmlElement("error")]
        [XmlText]
        public string Error { get; set; }
        /// <summary>
        /// A value representing the current type to help with assertions
        /// </summary>
        public string ElementType => this.GetType().Name;
        public XmlReportItem AddChildren(params XmlReportItem[] children)
        {
            Children.AddRange(children);
            return this;
        }
        public XmlReportItem WithError(string error)
        {
            Error = error;
            return this;
        }
    }

    [XmlRoot("root")]
    public class XmlReportRoot : XmlReportItem { }

    [XmlType("action")]
    public class XmlReportAction : XmlReportItem { }

    [XmlType("question")]
    public class XmlReportQuestion : XmlReportItem { }

    [XmlType("given")]
    public class XmlReportGiven : XmlReportItem { }

    [XmlType("when")]
    public class XmlReportWhen : XmlReportItem { }

    [XmlType("then")]
    public class XmlReportThen : XmlReportItem
    {
        [XmlAttribute("outcome")]
        public XmlReportThenOutcome Outcome { get; set; }
        [XmlElement("outcomeDetail")]
        [XmlText]
        public string OutcomeDetail { get; set; }

        public XmlReportThen WithOutcome(XmlReportThenOutcome outcome)
        {
            Outcome = outcome;
            return this;
        }

        public XmlReportThen WithOutcomeDetail(string details)
        {
            OutcomeDetail = details;
            return this;
        }
    }

    public enum XmlReportThenOutcome
    {
        pass,
        failed,
        error
    }

    public class XlmActionAttachment
    {
        [XmlAttribute("filepath")]
        public string FilePath { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
    }

    public class XmlDocumentObserverTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(XmlDocumentObserver));
        }

        [Theory, DomainAutoData]
        public void Sut_IsActionNotificationObserver(XmlDocumentObserver sut)
        {
            sut.Should().BeAssignableTo<IObserver<ActionNotification>>();
        }

        [Theory, DomainAutoData]
        public void Sut_IsActionFileAttachmentObserver(XmlDocumentObserver sut)
        {
            sut.Should().BeAssignableTo<IObserver<ActionFileAttachment>>();
        }

        public static IEnumerable<object[]> Notifications
        {
            get
            {
                var fixture = new Fixture().Customize(new DomainCustomization());
                T createItemGeneric<T>(
                    string name,
                    DateTimeOffset startDate,
                    int duration,
                    bool hasError,
                    List<XmlReportItem> children = null,
                    string error = null) where T : XmlReportItem, new()
                {
                    return new T() {
                        Name = name,
                        Duration = duration,
                        StartDate = startDate.ToString(CultureInfo.InvariantCulture),
                        EndDate = startDate.Add(TimeSpan.FromMilliseconds(duration)).ToString(CultureInfo.InvariantCulture),
                        HasError = hasError,
                        Children = children ?? new List<XmlReportItem>(),
                        Error = error
                    };
                }
                XmlReportItem createItem(
                    CommandType commandType,
                    string name,
                    DateTimeOffset startDate,
                    int duration,
                    bool hasError = false,
                    string error = null)
                {
                    if (commandType == CommandType.Action)
                    {
                        return createItemGeneric<XmlReportAction>(name, startDate, duration, hasError, error: error);
                    }
                    if (commandType == CommandType.Question)
                    {
                        return createItemGeneric<XmlReportQuestion>(name, startDate, duration, hasError, error: error);
                    }
                    throw new NotSupportedException($"{commandType} not supported");
                }
                var expectedDate = DateTimeOffset.Now;
                yield return new object[]
                {
                    "No notifications",
                    new ActionNotification[]{ },
                    createItemGeneric<XmlReportRoot>("Test", expectedDate, 0, false)
                };

                {
                    var action = fixture.Create<INamed>();
                    yield return new object[]
                    {
                        "Single notification",
                        new ActionNotification[]{
                            new ActionNotification(action, 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Action)),
                            new ActionNotification(action, 1, new AfterActionNotificationContent(TimeSpan.FromSeconds(1)))
                        },
                        createItemGeneric<XmlReportRoot>(
                            "Test",
                            expectedDate,
                            0,
                            false,
                            new List<XmlReportItem>()
                            {
                                createItem(CommandType.Action, action.Name, DateTimeOffset.MinValue, 1000)
                            })
                    };
                }

                {
                    var actions = fixture.CreateMany<(INamed action, CommandType commandType)>().ToArray();
                    yield return new object[]
                    {
                        "Multiple notifications",
                        actions.SelectMany((a, i) => new[]
                                {
                                    new ActionNotification(a.action, i + 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, a.commandType)),
                                    new ActionNotification(a.action, i + 1, new AfterActionNotificationContent(TimeSpan.FromSeconds(i)))
                                })
                               .ToArray(),
                        createItemGeneric<XmlReportRoot>(
                            "Test",
                            expectedDate,
                            0,
                            false,
                            actions.Select((a, i) => createItem(a.commandType, a.action.Name, DateTimeOffset.MinValue, i * 1000))
                                              .ToList())
                    };
                }

                {
                    var actions = fixture.CreateMany<(INamed action, CommandType commandType)>().ToArray();
                    yield return new object[]
                    {
                        "Nested notifications",
                        actions.Select((a,i) => (a.action, a.commandType, i))
                               .Reverse()
                               .Aggregate(
                                    ImmutableArray<ActionNotification>.Empty,
                                    (notifications, a) => notifications
                                        .Insert(0, new ActionNotification(a.action, a.i + 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, a.commandType)))
                                        .Add(new ActionNotification(a.action, a.i + 1, new AfterActionNotificationContent(TimeSpan.FromSeconds(a.i))))
                                ),
                        actions.Select((a,i) => (a.action, a.commandType, i))
                                                  .Select(a => createItem(a.commandType, a.action.Name, DateTimeOffset.MinValue, a.i * 1000))
                                                  .Reverse()
                                                  .Concat(new[]{ createItemGeneric<XmlReportRoot>("Test", expectedDate, 0, false)})
                                                  .Aggregate(
                                                    (child, parent) => {
                                                        parent.Children.Add(child);
                                                        return parent;
                                                    })
                    };
                }


                {
                    var exception = fixture.Create<Exception>();
                    var actions = fixture.CreateMany<(INamed action, CommandType commandType)>(4).ToArray();
                    yield return new object[]
                    {
                        "Error nested notifications",
                        actions.Select((a,i) => (a.action, a.commandType, exception, i))
                               .Reverse()
                               .Aggregate(
                                    ImmutableArray<ActionNotification>.Empty,
                                    (notifications, a) => notifications
                                        .Insert(0, new ActionNotification(a.action, a.i + 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, a.commandType)))
                                        .Add(new ActionNotification(a.action, a.i + 1,
                                                    new ExecutionErrorNotificationContent(a.exception, TimeSpan.FromSeconds(a.i))
                                                    ))
                                ),
                        actions.Select((a,i) => (a.action, a.commandType, exception, i))
                                                  .Select(a =>
                                                    createItem(
                                                        a.commandType,
                                                        a.action.Name,
                                                        DateTimeOffset.MinValue,
                                                        a.i * 1000,
                                                        true,
                                                        error: a.i < 3 ? null : exception.ToString())
                                                        )
                                                  .Reverse()
                                                  .Concat(new[]{ createItemGeneric<XmlReportRoot>("Test", expectedDate, 0, false)})
                                                  .Aggregate(
                                                    (child, parent) => {
                                                        parent.Children.Add(child);
                                                        return parent;
                                                    })
                    };
                }

                {
                    var question = fixture.Create<IQuestion<object>>();
                    var thenAction = new ThenAction<object, string>(question, _ => string.Empty);
                    yield return new object[]
                    {
                        "Verification",
                        new ActionNotification[]{
                            new ActionNotification(thenAction, 1, new BeforeThenNotificationContent(DateTimeOffset.MinValue, question)),
                            new ActionNotification(question, 2, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Question)),
                            new ActionNotification(question, 2, new AfterActionNotificationContent(TimeSpan.FromSeconds(1))),
                            new ActionNotification(thenAction, 1, new AfterThenNotificationContent(TimeSpan.FromSeconds(1), ThenOutcome.Pass))
                        },
                        createItemGeneric<XmlReportRoot>(
                            "Test",
                            expectedDate,
                            0,
                            false,
                            new List<XmlReportItem>()
                            {
                                createItemGeneric<XmlReportThen>(
                                    thenAction.Name,
                                    DateTimeOffset.MinValue,
                                    1000,
                                    false,
                                    new List<XmlReportItem>()
                                    {
                                        createItem(CommandType.Question, question.Name, DateTimeOffset.MinValue, 1000)
                                    })
                                    .WithOutcome(XmlReportThenOutcome.pass)
                            })
                    };
                }

                object[] createThenWithException(ThenOutcome outcome, XmlReportThenOutcome expectedOutcome)
                {
                    var question = fixture.Create<IQuestion<object>>();
                    var thenAction = new ThenAction<object, string>(question, _ => string.Empty);
                    var exception = fixture.Create<Exception>();
                    return new object[]
                    {
                        "Verification with failure (" + outcome.ToString() + ")",
                        new ActionNotification[]{
                            new ActionNotification(thenAction, 1, new BeforeThenNotificationContent(DateTimeOffset.MinValue, question)),
                            new ActionNotification(question, 2, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Question)),
                            new ActionNotification(question, 2, new AfterActionNotificationContent(TimeSpan.FromSeconds(1))),
                            new ActionNotification(thenAction, 1, new AfterThenNotificationContent(TimeSpan.FromSeconds(1), outcome, exception))
                        },
                        createItemGeneric<XmlReportRoot>(
                            "Test",
                            expectedDate,
                            0,
                            false,
                            new List<XmlReportItem>()
                            {
                                createItemGeneric<XmlReportThen>(
                                    thenAction.Name,
                                    DateTimeOffset.MinValue,
                                    1000,
                                    true,
                                    new List<XmlReportItem>()
                                    {
                                        createItem(CommandType.Question, question.Name, DateTimeOffset.MinValue, 1000)
                                    })
                                    .WithOutcome(expectedOutcome)
                                    .WithOutcomeDetail(exception.Message)
                            })
                    };
                }
                yield return createThenWithException(ThenOutcome.Failed, XmlReportThenOutcome.failed);
                yield return createThenWithException(ThenOutcome.Error, XmlReportThenOutcome.error);

                object[] createWhenThen<T>(ActionContext actionContext, CommandType commandType)
                    where T : XmlReportItem, new()
                {
                    var action = fixture.Create<INamed>();
                    return new object[]
                    {
                        $"Single notification with context ({actionContext}, {commandType})",
                        new ActionNotification[]{
                            new ActionNotification(action, 1, new BeforeFirstActionNotificationContent(DateTimeOffset.MinValue, actionContext)),
                            new ActionNotification(action, 2, new BeforeActionNotificationContent(DateTimeOffset.MinValue, commandType)),
                            new ActionNotification(action, 2, new AfterActionNotificationContent(TimeSpan.FromSeconds(1))),
                            new ActionNotification(action, 1, new AfterActionNotificationContent(TimeSpan.FromSeconds(1)))
                        },
                        createItemGeneric<XmlReportRoot>(
                            "Test",
                            expectedDate,
                            0,
                            false,
                            new List<XmlReportItem>()
                            {
                                createItemGeneric<T>(
                                    action.Name,
                                    DateTimeOffset.MinValue,
                                    1000,
                                    false,
                                    children: new List<XmlReportItem>()
                                    {
                                        createItem(commandType, action.Name, DateTimeOffset.MinValue, 1000)
                                    })
                            })
                    };
                }

                yield return createWhenThen<XmlReportGiven>(ActionContext.Given, CommandType.Action);
                yield return createWhenThen<XmlReportGiven>(ActionContext.Given, CommandType.Question);
                yield return createWhenThen<XmlReportWhen>(ActionContext.When, CommandType.Action);
                yield return createWhenThen<XmlReportWhen>(ActionContext.When, CommandType.Question);
            }
        }

        #region GetXmlDocument
        [Theory, MemberData(nameof(Notifications))]
        public void GetXmlDocument_ShouldReturnCorrectValue(
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
            string testName,
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
            ActionNotification[] notifications,
            XmlReportRoot expected)
        {
            // arrange            
            var fixture = new Fixture().Customize(new DomainCustomization());
            fixture.Register(() => DateTimeOffset.Parse(expected.StartDate, CultureInfo.InvariantCulture));
            var sut = fixture.Build<XmlDocumentObserver>()
                .FromFactory(new MethodInvoker(new GreedyConstructorQuery()))
                .Create();
            foreach (var notification in notifications)
            {
                sut.OnNext(notification);
            }
            // act
            var xmlDocument = sut.GetXmlDocument();
            // assert            
            XmlReportRoot actual = DeserializeXmlDocument(xmlDocument);
            AssertRootAreEqual(expected, actual);
        }

        [Theory, DomainAutoData]
        public void GetXmlDocument_WhenNotificationsDidNotEnd_ShouldThrow(
            XmlDocumentObserver sut,
            INamed action)
        {
            // arrange
            sut.OnNext(new ActionNotification(action, 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Action)));
            // act and assert
            new Action(() => sut.GetXmlDocument()).Should().ThrowExactly<InvalidOperationException>();
        }

        [Theory, DomainAutoData]
        public void GetXmlDocument_WhenCallingOnCompleted_ShouldReturnXmlDocument(
            [Frozen]DateTimeOffset date,
            [Greedy]XmlDocumentObserver sut)
        {
            //arrange
            sut.OnCompleted();
            //act
            var xmlDocument = sut.GetXmlDocument();
            //assert
            var actual = DeserializeXmlDocument(xmlDocument);
            AssertRootAreEqual(new XmlReportRoot() {
                Name = "Test",
                StartDate = date.ToString(CultureInfo.InvariantCulture),
                EndDate = date.ToString(CultureInfo.InvariantCulture)
            },
            actual);
        }

        [Theory, DomainAutoData]
        public void GetXmlDocument_WhenCallingOnError_ShouldReturnXmlDocument(
            [Frozen]DateTimeOffset date,
            [Greedy]XmlDocumentObserver sut,
            Exception exception)
        {
            //arrange
            sut.OnError(exception);
            //act
            var xmlDocument = sut.GetXmlDocument();
            //assert
            var actual = DeserializeXmlDocument(xmlDocument);
            AssertRootAreEqual(new XmlReportRoot() {
                Name = "Test",
                StartDate = date.ToString(CultureInfo.InvariantCulture),
                EndDate = date.ToString(CultureInfo.InvariantCulture)
            },
            actual);
        }

        public static XmlReportRoot DeserializeXmlDocument(XDocument xmlDocument)
        {
            var serializer = new XmlSerializer(typeof(XmlReportRoot), new[] { typeof(XmlReportAction), typeof(XmlReportQuestion) });
            var actual = (XmlReportRoot)serializer.Deserialize(xmlDocument.CreateReader());
            return actual;
        }

        [Theory]
        [DomainInlineAutoData(1)]
        [DomainInlineAutoData(5)]
        public void GetXmlDocument_WithAttachments_ShouldReturnCorrectValue(
            int count,
            DateTimeOffset startDate,
            [Frozen]DateTimeOffset date,
            [Greedy]XmlDocumentObserver sut,
            INamed named,
            IFixture fixture
            )
        {
            // arrange
            var attachments = fixture.CreateMany<ActionFileAttachment>(count).ToArray();
            sut.OnNext(new ActionNotification(named, 1, new BeforeActionNotificationContent(startDate, CommandType.Action)));
            // act           
            foreach (var attachment in attachments)
            {
                sut.OnNext(attachment);
            }
            sut.OnNext(new ActionNotification(named, 1, new AfterActionNotificationContent(TimeSpan.Zero)));
            // assert
            var expected = new XmlReportRoot() {
                Name = "Test",
                StartDate = date.ToString(CultureInfo.InvariantCulture),
                EndDate = date.ToString(CultureInfo.InvariantCulture),
                Children = new List<XmlReportItem>()
                            {
                    new XmlReportAction()
                    {
                        Name = named.Name,
                        Duration = 0,
                        StartDate = startDate.ToString(CultureInfo.InvariantCulture),
                        EndDate = startDate.ToString(CultureInfo.InvariantCulture),
                        HasError = false,
                        Attachments = attachments.Select(attachment =>
                            new XlmActionAttachment()
                            {
                                Description = attachment.Description,
                                FilePath = attachment.FilePath,
                            }).ToList()
                    }
                }
            };
            var actual = DeserializeXmlDocument(sut.GetXmlDocument());
            AssertRootAreEqual(expected, actual);
        }

#pragma warning disable xUnit1013 // Public method should be marked as test
        public static void AssertRootAreEqual(XmlReportRoot expected, XmlReportRoot actual)
#pragma warning restore xUnit1013 // Public method should be marked as test
        {
            actual.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }
        #endregion

        #region GetHtmlDocument
        [Theory, DomainAutoData]
        public void GetHtmlDocument_ShouldReturnCorrectValue(
            XmlDocumentObserver sut,
            IFixture fixture)
        {
            // arrange
            var actions = fixture.CreateMany<INamed>(6)
                                 .Select((action, i) => (action, commandType: i % 2 == 0 ? CommandType.Action : CommandType.Question))
                                 .ToArray();
            var notifications = actions
                               .Select((a, i) => (a.action, a.commandType, i))
                               .Reverse()
                               .Aggregate(
                                    ImmutableArray<ActionNotification>.Empty,
                                    (n, a) => n
                                        .Insert(0, new ActionNotification(a.action, a.i + 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, a.commandType)))
                                        .Add(new ActionNotification(a.action, a.i + 1, new AfterActionNotificationContent(TimeSpan.FromSeconds(a.i))))
                                );
            foreach (var notification in notifications)
            {
                sut.OnNext(notification);
            }
            // act
            var actual = sut.GetHtmlDocument();
            // assert            
            actual.Should()
                  .ContainAll(notifications.Select(a => a.Action.Name))
                  .And.StartWith("<!DOCTYPE html>");
            var questionCount = actual.Split(new[] { "class=\"question\"" }, StringSplitOptions.None).Length - 1;
            var expectedQuestionCount = actions.Count(a => a.commandType == CommandType.Question);
            questionCount.Should().Be(expectedQuestionCount, "Number of questions");
            var actionCount = actual.Split(new[] { "class=\"action\"" }, StringSplitOptions.None).Length - 1;
            var expectedActionCount = actions.Count(a => a.commandType == CommandType.Action);
            actionCount.Should().Be(expectedActionCount, "Number of actions");
        }

        [Theory]
        [DomainInlineAutoData(CommandType.Action, "action")]
        [DomainInlineAutoData(CommandType.Question, "question")]
        public void GetHtmlDocument_WithError_ShouldReturnCorrectErrorCount(
            CommandType commandType,
            string className,
            XmlDocumentObserver sut,
            INamed action,
            Exception exception)
        {
            //arrange
            sut.OnNext(new ActionNotification(action, 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, commandType)));
            sut.OnNext(new ActionNotification(action, 1, new ExecutionErrorNotificationContent(exception, TimeSpan.FromSeconds(1))));
            //act
            var actual = sut.GetHtmlDocument();
            //assert            
            var errorCount = actual.Split(new[] { $"class=\"{className} error\"" }, StringSplitOptions.None).Length - 1;
            errorCount.Should().Be(1);
        }

        [Theory]
        [DomainInlineAutoData(CommandType.Action)]
        [DomainInlineAutoData(CommandType.Question)]
        public void GetHtmlDocument_WithError_ShouldReturnCorrectError(
            CommandType commandType,
            XmlDocumentObserver sut,
            INamed action,
            Exception exception)
        {
            //arrange
            sut.OnNext(new ActionNotification(action, 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, commandType)));
            sut.OnNext(new ActionNotification(action, 1, new ExecutionErrorNotificationContent(exception, TimeSpan.FromSeconds(1))));
            //act
            var actual = sut.GetHtmlDocument();
            //assert            
            var exceptionCount = actual.Split(new[] { exception.ToString() }, StringSplitOptions.None).Length - 1;
            exceptionCount.Should().Be(1);
        }

        [Theory]
        [DomainAutoData]
        public void GetHtmlDocument_WithAttachments_ShouldContainImgElement(
            XmlDocumentObserver sut,
            INamed named,
            ActionFileAttachment attachment)
        {
            //arrange            
            sut.OnNext(new ActionNotification(named, 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Action)));
            sut.OnNext(attachment);
            sut.OnNext(new ActionNotification(named, 1, new AfterActionNotificationContent(TimeSpan.Zero)));
            //act
            var actual = sut.GetHtmlDocument();
            //assert
            actual.Should().Contain($"<img src=\"{attachment.FilePath}\" class=\"attachment\" />");
        }

        public XmlDocumentObserverTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        private readonly Xunit.Abstractions.ITestOutputHelper testOutputHelper;

        [Theory]
        [DomainAutoData]
        public void GetHtmlDocument_WithPassingVerification_ShouldContainImgElement(
            XmlDocumentObserver sut,
            ThenAction<object, string> thenAction)
        {
            //arrange  
            sut.OnNext(new ActionNotification(thenAction, 1, new BeforeThenNotificationContent(DateTimeOffset.MinValue, thenAction.Question)));
            sut.OnNext(new ActionNotification(thenAction.Question, 2, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Question)));
            sut.OnNext(new ActionNotification(thenAction.Question, 2, new AfterActionNotificationContent(TimeSpan.FromSeconds(1))));
            sut.OnNext(new ActionNotification(thenAction, 1, new AfterThenNotificationContent(TimeSpan.FromSeconds(1), ThenOutcome.Pass)));
            //act
            var actual = sut.GetHtmlDocument();
            testOutputHelper.WriteLine(actual);
            //assert
            actual.Should().Contain("class=\"action-context then-pass\">")
                  .And.Contain(thenAction.Name)
                  .And.Contain("class=\"question\"");
        }

        [Theory]
        [DomainInlineAutoData(ThenOutcome.Error)]
        [DomainInlineAutoData(ThenOutcome.Failed)]
        public void GetHtmlDocument_WithFailingVerification_ShouldContainImgElement(
            ThenOutcome outcome,
            Exception exception,
            XmlDocumentObserver sut,
            ThenAction<object, string> thenAction)
        {
            //arrange  
            sut.OnNext(new ActionNotification(thenAction, 1, new BeforeThenNotificationContent(DateTimeOffset.MinValue, thenAction.Question)));
            sut.OnNext(new ActionNotification(thenAction.Question, 2, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Question)));
            sut.OnNext(new ActionNotification(thenAction.Question, 2, new AfterActionNotificationContent(TimeSpan.FromSeconds(1))));
            sut.OnNext(new ActionNotification(thenAction, 1, new AfterThenNotificationContent(TimeSpan.FromSeconds(1), outcome, exception)));
            //act
            var actual = sut.GetHtmlDocument();
            testOutputHelper.WriteLine(actual);
            //testOutputHelper.WriteLine(sut.GetXmlDocument().ToString(SaveOptions.None));
            //assert
            actual.Should().Contain("class=\"action-context then-fail error\">")
                  .And.Contain(thenAction.Name)
                  .And.Contain("class=\"question\"")
                  .And.Contain("class=\"then-fail-detail error\"")
                  .And.Contain(exception.Message);
        }

        [Theory]
        [DomainInlineAutoData(ActionContext.Given)]
        [DomainInlineAutoData(ActionContext.When)]
        public void GetHtmlDocument_WithGivenWhen_ShouldReturnCorrectValue(
            ActionContext actionContext,
            XmlDocumentObserver sut,
            IFixture fixture)
        {
            // arrange
            var actions = fixture.CreateMany<IAction<Unit>>(3)
                                 .ToArray();
            var notifications = actions
                               .Select((a, i) => (action: a, i: i + 1))
                               .Reverse()
                               .Aggregate(
                                    ImmutableArray<ActionNotification>.Empty,
                                    (n, a) => n
                                        .Insert(0, new ActionNotification(a.action, a.i + 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Action)))
                                        .Add(new ActionNotification(a.action, a.i + 1, new AfterActionNotificationContent(TimeSpan.FromSeconds(a.i))))
                                );
            var firstAction = new CommandAction<Unit>(actions[0], actionContext);
            sut.OnNext(new ActionNotification(firstAction, 1, new BeforeFirstActionNotificationContent(DateTimeOffset.MinValue, actionContext)));
            foreach (var notification in notifications)
            {
                sut.OnNext(notification);
            }
            sut.OnNext(new ActionNotification(firstAction, 1, new AfterActionNotificationContent(TimeSpan.FromSeconds(1))));
            // act
            var actual = sut.GetHtmlDocument();
            // assert            
            actual.Should()
                  .ContainAll(notifications.Select(a => a.Action.Name))
                  .And.StartWith("<!DOCTYPE html>");
            var expectedClass = actionContext.ToString().ToLower();
            var classesCount = CountStringOccurences(actual, $"class=\"action-context first-action-{expectedClass}\"");
            classesCount.Should().Be(1);
            var titleCount = CountStringOccurences(actual, firstAction.Name);
            titleCount.Should().Be(1);

        }
        #endregion

        private static int CountStringOccurences(string input, string value)
        {
            return input.Split(new[] { value }, StringSplitOptions.None).Length - 1;
        }
    }
}