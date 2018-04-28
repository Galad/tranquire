using OpenQA.Selenium;
using System;
using System.IO;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// An observer of <see cref="ScreenshotInfo"/> that saves the screenshots in the given directory when <see cref="OnNext(ScreenshotInfo)"/> is called
    /// </summary>
    public class SaveScreenshotsToFileOnNext : IObserver<ScreenshotInfo>
    {
        private readonly string _directory;

        /// <summary>
        /// Creates a new instance of <see cref="SaveScreenshotsToFileOnNext"/>
        /// </summary>
        /// <param name="directory">The directory where the screenshots will be saved</param>
        public SaveScreenshotsToFileOnNext(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            _directory = directory;
        }

        /// <inheritsdoc />
        public void OnCompleted()
        {
            // Not used
        }

        /// <inheritsdoc />
        public void OnError(Exception error)
        {
            // Not used
        }

        /// <inheritsdoc />
        public void OnNext(ScreenshotInfo value)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
            var filename = Path.Combine(_directory, value.FileName + ".jpg");
            value.Screenshot.SaveAsFile(filename, ScreenshotImageFormat.Jpeg);
        }
    }
}
