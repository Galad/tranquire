using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tranquire.Reporting
{
    /// <summary>
    /// A <see cref="IObserver{ActionNotification}"/> implementation that writes notifications in a XmlDocument
    /// </summary>
    public partial class XmlDocumentObserver : IObserver<ActionNotification>
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

        public XmlDocumentObserver()
        {
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(ActionNotification value)
        {
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
                default:
                    break;
            }
        }

        private void HandleExecutionErrorNotificationContent(ExecutionErrorNotificationContent content)
        {
            var item = _items.Pop();
            item.EndDate = DateTimeOffset.Now;
            item.Error = content.Exception;
        }

        private void HandleAfterActionExecution(AfterActionNotificationContent content)
        {
            var item = _items.Pop();
            item.EndDate = item.StartDate.Add(content.Duration);            
        }

        private void HandleBeforeActionExecution(BeforeActionNotificationContent beforeActionNotificationContent, INamed named)
        {
            var currentItem = CurrentItem;
            var newItem = CreateItem(beforeActionNotificationContent.CommandType);
            newItem.StartDate = DateTimeOffset.Now;
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
                case CommandType.Question:
                    return new TranquireXmlReportQuestion();
                default:
                    throw new NotSupportedException("The command type " + commandType.ToString() + " is not supported");
            }
        }

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
                if(item is TranquireXmlReportDocument)
                {
                    return "root";
                }
                if(item is TranquireXmlReportAction)
                {
                    return "action";
                }
                if (item is TranquireXmlReportQuestion)
                {
                    return "question";
                }
                return "";
            }
            var element = new XElement(
                elementName(),
                new XAttribute("start-date", item.StartDate),
                new XAttribute("end-date", item.EndDate),
                new XAttribute("duration", (int)item.Duration.TotalMilliseconds),
                new XAttribute("name", item.Name),
                item.Children.Select(GetElement)
                );
            return element;
        }
    }
}
