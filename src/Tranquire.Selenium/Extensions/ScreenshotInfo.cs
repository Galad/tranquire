using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// Represent a screenshot that was taken with Selenium
    /// </summary>
    public class ScreenshotInfo
    {
        /// <summary>
        /// Creates a new instance of <see cref="ScreenshotInfo"/>
        /// </summary>
        /// <param name="screenshot">The screenshot</param>
        /// <param name="fileName">The screenshot file name</param>
        public ScreenshotInfo(Screenshot screenshot, string fileName)
        {
            Screenshot = screenshot ?? throw new ArgumentNullException(nameof(screenshot));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        /// <summary>
        /// Gets the screenshot
        /// </summary>
        public Screenshot Screenshot { get; }
        /// <summary>
        /// Gets the screenshot file name
        /// </summary>
        public string FileName { get; }
    }
}
