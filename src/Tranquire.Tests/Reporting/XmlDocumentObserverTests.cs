using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
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
        public List<XmlReportItem> Children { get; set; } = new List<XmlReportItem>();
    }

    [XmlRoot("root")]
    public class XmlReportRoot : XmlReportItem { }

    [XmlType("action")]
    public class XmlReportAction : XmlReportItem { }

    [XmlType("question")]
    public class XmlReportQuestion : XmlReportItem { }

    public class XmlDocumentObserverTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(XmlDocumentObserver));
        }

        public static IEnumerable<object[]> Notifications
        {
            get
            {
                var fixture = new Fixture().Customize(new DomainCustomization());
                XmlReportItem createItemGeneric<T>(
                    string name,
                    DateTimeOffset startDate,
                    int duration,
                    bool hasError) where T : XmlReportItem, new()
                {
                    return new T()
                    {
                        Name = name,
                        Duration = duration,
                        StartDate = startDate.ToString(CultureInfo.InvariantCulture),
                        EndDate = startDate.Add(TimeSpan.FromMilliseconds(duration)).ToString(CultureInfo.InvariantCulture),
                        HasError = hasError
                    };
                }
                XmlReportItem createItem(
                    CommandType commandType,
                    string name,
                    DateTimeOffset startDate,
                    int duration,
                    bool hasError = false)
                {
                    if (commandType == CommandType.Action)
                    {
                        return createItemGeneric<XmlReportAction>(name, startDate, duration, hasError);
                    }
                    if (commandType == CommandType.Question)
                    {
                        return createItemGeneric<XmlReportQuestion>(name, startDate, duration, hasError);
                    }
                    throw new NotSupportedException($"{commandType} not supported");
                }

                yield return new object[]
                {
                    "No notifications",
                    new ActionNotification[]{ },
                    new XmlReportRoot()
                    {
                        Name = "Test"
                    }
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
                        new XmlReportRoot()
                        {
                            Name = "Test",
                            Children = new List<XmlReportItem>()
                            {
                                createItem(CommandType.Action, action.Name, DateTimeOffset.MinValue, 1000)
                            }
                        }
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
                        new XmlReportRoot()
                        {
                            Name = "Test",
                            Children = actions.Select((a, i) => createItem(a.commandType, a.action.Name, DateTimeOffset.MinValue, i * 1000))
                                              .ToList()
                        }
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
                                                  .Concat(new[]{ new XmlReportRoot() { Name = "Test" }})
                                                  .Aggregate(
                                                    (child, parent) => {
                                                        parent.Children.Add(child);
                                                        return parent;
                                                    })
                    };
                }


                {
                    var actions = fixture.CreateMany<(INamed action, Exception exception, CommandType commandType)>(4).ToArray();
                    yield return new object[]
                    {
                        "Error nested notifications",
                        actions.Select((a,i) => (a.action, a.commandType, a.exception, i))
                               .Reverse()
                               .Aggregate(
                                    ImmutableArray<ActionNotification>.Empty,
                                    (notifications, a) => notifications
                                        .Insert(0, new ActionNotification(a.action, a.i + 1, new BeforeActionNotificationContent(DateTimeOffset.MinValue, a.commandType)))
                                        .Add(new ActionNotification(a.action, a.i + 1,
                                                    a.i < 3 ?
                                                    new AfterActionNotificationContent(TimeSpan.FromSeconds(a.i)) :
                                                    (IActionNotificationContent)new ExecutionErrorNotificationContent(a.exception, TimeSpan.FromSeconds(a.i))
                                                    ))
                                ),
                        actions.Select((a,i) => (a.action, a.commandType, i))
                                                  .Select(a => createItem(a.commandType, a.action.Name, DateTimeOffset.MinValue, a.i * 1000, a.i == 3))
                                                  .Reverse()
                                                  .Concat(new[]{ new XmlReportRoot() { Name = "Test" }})
                                                  .Aggregate(
                                                    (child, parent) => {
                                                        parent.Children.Add(child);
                                                        return parent;
                                                    })
                    };
                }
            }
        }

        [Theory, MemberData(nameof(Notifications))]
        public void GetXmlDocument_ShouldReturnCorrectValue(
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
            string testName,
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
            ActionNotification[] notifications,
            XmlReportRoot expected)
        {
            // arrange
            var sut = new Fixture().Create<XmlDocumentObserver>();
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
            new Action(() => sut.GetHtmlDocument()).Should().ThrowExactly<InvalidOperationException>();
        }

        [Theory, DomainAutoData]
        public void GetXmlDocument_WhenCallingOnCompleted_ShouldReturnXmlDocument(XmlDocumentObserver sut)
        {
            //arrange
            sut.OnCompleted();
            //act
            var xmlDocument = sut.GetXmlDocument();
            //assert
            var actual = DeserializeXmlDocument(xmlDocument);
            AssertRootAreEqual(new XmlReportRoot() { Name = "Test" }, actual);
        }

        [Theory, DomainAutoData]
        public void GetXmlDocument_WhenCallingOnError_ShouldReturnXmlDocument(XmlDocumentObserver sut, Exception exception)
        {
            //arrange
            sut.OnError(exception);
            //act
            var xmlDocument = sut.GetXmlDocument();
            //assert
            var actual = DeserializeXmlDocument(xmlDocument);
            AssertRootAreEqual(new XmlReportRoot() { Name = "Test" }, actual);
        }

        private static XmlReportRoot DeserializeXmlDocument(XDocument xmlDocument)
        {
            var serializer = new XmlSerializer(typeof(XmlReportRoot), new[] { typeof(XmlReportAction), typeof(XmlReportQuestion) });
            var actual = (XmlReportRoot)serializer.Deserialize(xmlDocument.CreateReader());
            return actual;
        }

        private static void AssertRootAreEqual(XmlReportRoot expected, XmlReportRoot actual)
        {
            actual.Should().BeEquivalentTo(expected, o => o.Excluding(i => i.StartDate)
                                                                       .Excluding(i => i.EndDate)
                                                                       .Excluding(i => i.Duration));
        }

        [Theory, DomainAutoData]
        public void GetHtmlDocument_ShouldReturnCorrectValue(
            XmlDocumentObserver sut,
            (INamed action, CommandType commandType)[] actions)
        {
            // arrange
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
            
        }
    }
}
