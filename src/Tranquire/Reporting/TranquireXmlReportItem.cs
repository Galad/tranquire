using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Tranquire.Reporting
{
    internal abstract class TranquireXmlReportItem
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public TimeSpan Duration => EndDate.Subtract(StartDate);
        public string Name { get; set; }
        public Exception Error { get; set; }
        public bool HasError => Error != null;
        public List<TranquireXmlReportItem> Children { get; } = new List<TranquireXmlReportItem>();
        public List<ActionFileAttachment> Attachments { get; } = new List<ActionFileAttachment>();
    }

    internal class TranquireXmlReportDocument : TranquireXmlReportItem { }
    internal class TranquireXmlReportAction : TranquireXmlReportItem { }
    internal class TranquireXmlReportQuestion : TranquireXmlReportItem { }
    internal class TranquireXmlReportThen : TranquireXmlReportItem
    {
        public ThenOutcome Outcome { get; set; }
        public string OutcomeDetail { get; set; }
    }
}
