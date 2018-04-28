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
        private readonly List<ScreenshotInfo> screenshots;

        /// <summary>
        /// Creates a new instance of <see cref="SaveScreenshotsToFileOnComplete"/>
        /// </summary>
        /// <param name="directory">The directory where the screenshots will be saved</param>
        public SaveScreenshotsToFileOnComplete(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            screenshots = new List<ScreenshotInfo>();
            Directory = directory;
        }

        /// <summary>
        /// Gets the directory
        /// </summary>
        public string Directory { get; }

        /// <inheritsdoc />
        public void OnCompleted()
        {
            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }
            foreach(var screenshot in screenshots)
            {
                var filename = Path.Combine(Directory, screenshot.FileName + ".jpg");
                screenshot.Screenshot.SaveAsFile(filename, ScreenshotImageFormat.Jpeg);
            }
        }

        /// <inheritsdoc />
        public void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }
        }

        /// <inheritsdoc />
        public void OnNext(ScreenshotInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            screenshots.Add(value);
        }
    }
}
