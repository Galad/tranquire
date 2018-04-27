using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium.Extensions
{
    public class SaveScreenshotsToFileOnComplete : IObserver<ScreenshotInfo>
    {
        private readonly Queue<ScreenshotInfo> _queue;
        private readonly string directory;

        public SaveScreenshotsToFileOnComplete(string directory)
        {
            this._queue = new Queue<ScreenshotInfo>();
            this.directory = directory;
        }

        public void OnCompleted()
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            while (_queue.Count > 0)
            {
                var screenshot = _queue.Dequeue();
                var filename = Path.Combine(directory, screenshot.FileName + ".jpg");
                screenshot.Screenshot.SaveAsFile(filename, ScreenshotImageFormat.Jpeg);
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(ScreenshotInfo value)
        {
            _queue.Enqueue(value);
        }
    }
}
