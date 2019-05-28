using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Tranquire.Reporting
{
    /// <summary>
    /// A <see cref="IObserver{ActionNotification}"/> implementation that writes notifications in a XmlDocument
    /// </summary>
    public partial class XmlDocumentObserver : IObserver<ActionNotification>, IObserver<ActionFileAttachment>
    {
        /// <summary>
        /// Creates a new instance of <see cref="XmlDocumentObserver"/>
        /// </summary>
        public XmlDocumentObserver(IMeasureDuration measureDuration)
        {
            MeasureDuration = measureDuration ?? throw new ArgumentNullException(nameof(measureDuration));
        }

        /// <summary>
        /// Creates a new instance of <see cref="XmlDocumentObserver"/>
        /// </summary>
        public XmlDocumentObserver():this(new DefaultMeasureDuration())
        {
        }

        private readonly Stack<TranquireXmlReportItem> _items = new Stack<TranquireXmlReportItem>();

        private TranquireXmlReportItem CurrentItem
        {
            get
            {
                if (_items.Count == 0)
                {
                    _items.Push(new TranquireXmlReportDocument()
                    {
                        StartDate = MeasureDuration.Now,
                        Name = "Test"
                    });
                }
                return _items.Peek();
            }
        }

        /// <summary>
        /// Gets the object that provides the current date
        /// </summary>
        public IMeasureDuration MeasureDuration { get; }

        /// <inheritsdoc />
        public void OnCompleted()
        {
            // not used
        }

        /// <inheritsdoc />
        public void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }
            // not used
        }

        /// <inheritsdoc />
        public void OnNext(ActionNotification value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            switch (value.Content.NotificationContentType)
            {
                case ActionNotificationContentType.BeforeActionExecution:
                    HandleBeforeActionExecution(value.Content as BeforeActionNotificationContent, value.Action);
                    break;
                case ActionNotificationContentType.BeforeFirstActionExecution:
                    HandleBeforeFirstActionExecution(value.Content as BeforeFirstActionNotificationContent, value.Action);
                    break;
                case ActionNotificationContentType.AfterActionExecution:
                    HandleAfterActionExecution(value.Content as AfterActionNotificationContent);
                    break;
                case ActionNotificationContentType.ExecutionError:
                    HandleExecutionErrorNotificationContent(value.Content as ExecutionErrorNotificationContent);
                    break;
                case ActionNotificationContentType.BeforeThen:
                    (value.Content as INotificationHandler).Handle(this, value.Action);
                    break;
                case ActionNotificationContentType.AfterThen:
                    HandleAfterThen(value.Content as AfterThenNotificationContent);
                    break;
            }
        }

        private void HandleAfterThen(AfterThenNotificationContent afterThenNotificationContent)
        {
            var item = _items.Pop() as TranquireXmlReportThen;
            item.EndDate = item.StartDate + afterThenNotificationContent.Duration;
            item.Outcome = afterThenNotificationContent.Outcome;
            item.Error = afterThenNotificationContent.Exception;
        }

        internal void HandleBeforeThen(BeforeThenNotificationContent beforeThen, INamed named)
        {
            var currentItem = CurrentItem;
            var newItem = new TranquireXmlReportThen()
            {
                StartDate = beforeThen.StartDate,
                Name = named.Name                
            };
            currentItem.Children.Add(newItem);
            _items.Push(newItem);
        }

        private void HandleExecutionErrorNotificationContent(ExecutionErrorNotificationContent content)
        {
            var item = _items.Pop();
            item.EndDate = item.StartDate + content.Duration;
            item.Error = content.Exception;
        }

        private void HandleAfterActionExecution(AfterActionNotificationContent content)
        {
            var item = _items.Pop();
            item.EndDate = item.StartDate + content.Duration;
        }

        private void HandleBeforeActionExecution(BeforeActionNotificationContent beforeActionNotificationContent, INamed named)
        {
            var currentItem = CurrentItem;
            var newItem = CreateItem(beforeActionNotificationContent.CommandType);
            newItem.StartDate = beforeActionNotificationContent.StartDate;
            newItem.Name = named.Name;
            currentItem.Children.Add(newItem);
            _items.Push(newItem);
        }

        private TranquireXmlReportItem CreateItem(CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.Action:
                    return new TranquireXmlReportAction();
                default:
                    return new TranquireXmlReportQuestion();
            }
        }

        private void HandleBeforeFirstActionExecution(
            BeforeFirstActionNotificationContent beforeFirstActionNotificationContent,
            INamed named)
        {
            var currentItem = CurrentItem;
            var newItem = CreateItem(beforeFirstActionNotificationContent.ActionContext);
            newItem.StartDate = beforeFirstActionNotificationContent.StartDate;
            newItem.Name = named.Name;
            currentItem.Children.Add(newItem);
            _items.Push(newItem);
        }

        private TranquireXmlReportItem CreateItem(ActionContext actionContext)
        {
            switch (actionContext)
            {
                case ActionContext.Given:
                    return new TranquireXmlReportGiven();
                default:
                    return new TranquireXmlReportWhen();
            }
        }

        /// <summary>
        /// Returns a <see cref="XDocument"/> instance that contains the test report in a XML format.
        /// </summary>
        /// <returns></returns>
        public XDocument GetXmlDocument()
        {
            if (_items.Count > 1)
            {
                throw new InvalidOperationException("The run did not finish, the XML document cannot be created");
            }

            var item = CurrentItem;
            item.EndDate = MeasureDuration.Now;
            var document = new XDocument(GetElement(item));
            return document;
        }

        private XElement GetElement(TranquireXmlReportItem item)
        {            
            var content = new List<object>()
            {
                new XAttribute("start-date", item.StartDate.ToString(CultureInfo.InvariantCulture)),
                new XAttribute("end-date", item.EndDate.ToString(CultureInfo.InvariantCulture)),
                new XAttribute("duration", (int)item.Duration.TotalMilliseconds),
                new XAttribute("name", item.Name),
                new XAttribute("has-error", item.HasError),                
                item.Children.Select(GetElement)
            };           
            if(item.Attachments.Count > 0)
            {
                content.Add(new XElement("attachments", item.Attachments.Select(getAttachment)));
            }
            if(item is TranquireXmlReportThen then)
            {
                content.Add(new XAttribute("outcome", then.Outcome.ToString().ToLower()));
                if (then.Outcome != ThenOutcome.Pass)
                {
                    content.Add(new XElement("outcomeDetail", new XCData(then.Error.Message)));
                }
            }            
            else if (item.HasError && !hasExceptionInChildren(item.Error, item.Children))
            {
                content.Add(new XElement("error", new XCData(item.Error.ToString())));
            }
            var element = new XElement(
                elementName(),
                content.ToArray()
                );
            return element;

            string elementName()
            {
                switch (item)
                {
                    case TranquireXmlReportQuestion _:
                        return "question";
                    case TranquireXmlReportAction _:
                        return "action";
                    case TranquireXmlReportThen _:
                        return "then";
                    case TranquireXmlReportGiven _:
                        return "given";
                    case TranquireXmlReportWhen _:
                        return "when";
                    default:
                        return "root";
                }
            }

            XElement getAttachment(ActionFileAttachment attachment)
            {
                return new XElement(
                    "attachment",
                    new XAttribute("filepath", attachment.FilePath),
                    new XAttribute("description", attachment.Description)
                    );
            }

            bool hasExceptionInChildren(Exception ex, IEnumerable<TranquireXmlReportItem> items)
            {
                return items.Any(i => i.Error == ex || hasExceptionInChildren(ex, i.Children));
            }
        }
        
        /// <inheritdoc />
        public void OnNext(ActionFileAttachment value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            CurrentItem.Attachments.Add(value);
        }
    }
}
