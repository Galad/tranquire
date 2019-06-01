using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tranquire.Reporting
{
    internal abstract class TranquireXmlReportItem
    {
        private ImmutableList<TranquireXmlReportItem> _children = ImmutableList<TranquireXmlReportItem>.Empty;
        private ImmutableList<ActionFileAttachment> _attachments = ImmutableList<ActionFileAttachment>.Empty;

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public TimeSpan Duration => EndDate.Subtract(StartDate);
        public string Name { get; set; }
        public Exception Error { get; set; }
        public bool HasError => Error != null;
        public ImmutableList<TranquireXmlReportItem> Children => _children;
        public ImmutableList<ActionFileAttachment> Attachments => _attachments;

        public void Add(TranquireXmlReportItem item) => _children = Children.Add(item);

        public void Add(ActionFileAttachment item) => _attachments = _attachments.Add(item);
    }

    internal class TranquireXmlReportDocument : TranquireXmlReportItem { }
    internal class TranquireXmlReportAction : TranquireXmlReportItem { }
    internal class TranquireXmlReportQuestion : TranquireXmlReportItem { }
    internal class TranquireXmlReportThen : TranquireXmlReportItem
    {
        public ThenOutcome Outcome { get; set; }
    }
    internal class TranquireXmlReportGiven : TranquireXmlReportItem { }
    internal class TranquireXmlReportWhen : TranquireXmlReportItem { }
}
