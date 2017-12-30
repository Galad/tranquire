namespace Tranquire.Reporting
{
    /// <summary>
    /// Represents information about a screenshot being taken during an action
    /// </summary>
    public class ScreenshotNotification
    {
        /// <summary>
        /// The path to the created screenshot
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ScreenshotNotification"/>
        /// </summary>
        /// <param name="filePath">Path to the created file</param>
        public ScreenshotNotification(string filePath)
        {
            FilePath = filePath;

        }
    }

}
