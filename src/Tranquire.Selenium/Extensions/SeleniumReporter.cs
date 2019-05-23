using System;
using System.Xml.Linq;
using Tranquire.Reporting;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// This class can retrieve reports and save screenshots
    /// </summary>
    public sealed class SeleniumReporter : ISeleniumReporter
    {
        /// <summary>
        /// Gets the XmlDocumentObserver
        /// </summary>
        public XmlDocumentObserver XmlDocumentObserver { get; }
        /// <summary>
        /// Gets the screenshots observer
        /// </summary>
        public IObserver<ScreenshotInfo> ScreenshotInfoObserver { get; }
        
        /// <summary>
        /// Creates a new instance of <see cref="SeleniumReporter"/>
        /// </summary>
        /// <param name="xmlDocumentObserver"></param>
        /// <param name="screenshotInfoObserver"></param>
        public SeleniumReporter(XmlDocumentObserver xmlDocumentObserver, IObserver<ScreenshotInfo> screenshotInfoObserver)
        {
            XmlDocumentObserver = xmlDocumentObserver ?? throw new ArgumentNullException(nameof(xmlDocumentObserver));
            ScreenshotInfoObserver = screenshotInfoObserver ?? throw new ArgumentNullException(nameof(screenshotInfoObserver));
        }

#if NET45
        /// <inheritdoc />
        public string GetHtmlDocument()
        {
            return XmlDocumentObserver.GetHtmlDocument();
        }
#endif

        /// <inheritdoc />
        public XDocument GetXmlDocument()
        {
            return XmlDocumentObserver.GetXmlDocument();
        }

        /// <inheritdoc />
        public void SaveScreenshots()
        {
            ScreenshotInfoObserver.OnCompleted();
        }
    }
}
