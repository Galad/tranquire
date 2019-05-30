using System;
using System.IO;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// An observer of <see cref="ScreenshotInfo"/> that saves the screenshots in the given directory when <see cref="OnNext(ScreenshotInfo)"/> is called
    /// </summary>
    public class SaveScreenshotsToFileOnNext : IObserver<ScreenshotInfo>
    {

        /// <summary>
        /// Creates a new instance of <see cref="SaveScreenshotsToFileOnNext"/>
        /// </summary>
        /// <param name="directory">The directory where the screenshots will be saved</param>
        /// <param name="format">The format which the screenshots are saved</param>
        public SaveScreenshotsToFileOnNext(string directory, ScreenshotFormat format)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            Directory = directory;
            Format = format ?? throw new ArgumentNullException(nameof(format));
        }

        /// <summary>
        /// Gets the directory
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// Gets the format which the screenshots are saved
        /// </summary>
        public ScreenshotFormat Format { get; }

        /// <inheritsdoc />
        public void OnCompleted()
        {
            // Not used
        }

        /// <inheritsdoc />
        public void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }
            // Not used
        }

        /// <inheritsdoc />
        public void OnNext(ScreenshotInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }
            var filename = Path.Combine(Directory, value.FileName + Format.Extension);
            value.Screenshot.SaveAsFile(filename, Format.Format);
        }
    }
}
