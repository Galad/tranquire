﻿using System.Xml.Linq;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Provides methods to handle reports for Selenium
    /// </summary>
    public interface ISeleniumReporter
    {
        /// <summary>
        /// Returns a report in an XML document format
        /// </summary>
        /// <returns></returns>
        XDocument GetXmlDocument();
        /// <summary>
        /// Returns a report in an HTML format
        /// </summary>
        /// <returns></returns>
        string GetHtmlDocument();
        /// <summary>
        /// Save the screenshots taken during the run to the disk
        /// </summary>
        void SaveScreenshots();
    }
}
