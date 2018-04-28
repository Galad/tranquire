using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// An observer of <see cref="ScreenshotInfo"/> that saves the screenshots in the given directory when <see cref="OnCompleted"/> is called
    /// </summary>
    public class SaveScreenshotsToFileOnComplete : IObserver<ScreenshotInfo>
    {
        private readonly Queue<ScreenshotInfo> _queue;
        private readonly string _directory;

        /// <summary>
        /// Creates a new instance of <see cref="SaveScreenshotsToFileOnComplete"/>
        /// </summary>
        /// <param name="directory">The directory where the screenshots will be saved</param>
        public SaveScreenshotsToFileOnComplete(string directory)
        {
            _queue = new Queue<ScreenshotInfo>();
            _directory = directory;
        }

        /// <inheritsdoc />
        public void OnCompleted()
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
            while (_queue.Count > 0)
            {
                var screenshot = _queue.Dequeue();
                var filename = Path.Combine(_directory, screenshot.FileName + ".jpg");
                screenshot.Screenshot.SaveAsFile(filename, ScreenshotImageFormat.Jpeg);
            }
        }

        /// <inheritsdoc />
        public void OnError(Exception error)
        {
            // Not used
        }

        /// <inheritsdoc />
        public void OnNext(ScreenshotInfo value)
        {
            _queue.Enqueue(value);
        }
    }
}
