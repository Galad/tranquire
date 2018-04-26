﻿using OpenQA.Selenium;
using System;
using System.IO;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium
{
    public class SaveScreenshotsToFileOnNext : IObserver<ScreenshotInfo>
    {
        private readonly string directory;

        public SaveScreenshotsToFileOnNext(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.directory = directory;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(ScreenshotInfo value)
        {
            var filename = Path.Combine(directory, value.FileName + ".jpg");
            value.Screenshot.SaveAsFile(filename, ScreenshotImageFormat.Jpeg);
        }
    }
}
