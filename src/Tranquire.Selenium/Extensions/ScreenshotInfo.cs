using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquire.Selenium.Extensions
{
    public class ScreenshotInfo
    {
        public ScreenshotInfo(Screenshot screenshot, string fileName)
        {
            Screenshot = screenshot ?? throw new ArgumentNullException(nameof(screenshot));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public Screenshot Screenshot { get; }
        public string FileName { get; }
    }
}
