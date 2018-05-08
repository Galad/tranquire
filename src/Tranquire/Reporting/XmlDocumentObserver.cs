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
        private readonly Stack<TranquireXmlReportItem> _items = new Stack<TranquireXmlReportItem>();

        private TranquireXmlReportItem CurrentItem
        {
            get
            {
                if (_items.Count == 0)
                {
                    _items.Push(new TranquireXmlReportDocument()
                    {
                        StartDate = DateTimeOffset.Now,
                        Name = "Test"
                    });
                }
                return _items.Peek();
            }
        }

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
                case ActionNotificationContentType.AfterActionExecution:
                    HandleAfterActionExecution(value.Content as AfterActionNotificationContent);
                    break;
                case ActionNotificationContentType.ExecutionError:
                    HandleExecutionErrorNotificationContent(value.Content as ExecutionErrorNotificationContent);
                    break;
            }
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
            var document = new XDocument(GetElement(item));
            return document;
        }

        private XElement GetElement(TranquireXmlReportItem item)
        {
            string elementName()
            {
                if (item is TranquireXmlReportDocument)
                {
                    return "root";
                }
                if (item is TranquireXmlReportAction)
                {
                    return "action";
                }
                return "question";
            }
            XElement getAttachment(ActionFileAttachment attachment)
            {
                return new XElement(
                    "attachment",
                    new XAttribute("filepath", attachment.FilePath),
                    new XAttribute("description", attachment.Description)
                    );
            }
            var element = new XElement(
                elementName(),
                new XAttribute("start-date", item.StartDate.ToString(CultureInfo.InvariantCulture)),
                new XAttribute("end-date", item.EndDate.ToString(CultureInfo.InvariantCulture)),
                new XAttribute("duration", (int)item.Duration.TotalMilliseconds),
                new XAttribute("name", item.Name),
                new XAttribute("has-error", item.HasError),
                new XElement("attachments", item.Attachments.Select(getAttachment)),
                item.Children.Select(GetElement)      
                );
            return element;
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
