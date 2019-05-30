using System;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// Represent an observer the render a <see cref="ScreenshotInfo"/> to a string and send to to the inner observer
    /// </summary>
    public sealed class RenderedScreenshotInfoObserver : IObserver<ScreenshotInfo>
    {
        /// <summary>
        /// Creates a new instance of <see cref="RenderedScreenshotInfoObserver"/>
        /// </summary>
        /// <param name="observer">The observer to render to</param>
        /// <param name="format">The format which the screenshots are saved</param>
        public RenderedScreenshotInfoObserver(IObserver<string> observer, ScreenshotFormat format)
        {
            this.Observer = observer ?? throw new ArgumentNullException(nameof(observer));
            Format = format ?? throw new ArgumentNullException(nameof(format));
        }

        /// <summary>
        /// Gets the observer
        /// </summary>
        public IObserver<string> Observer { get; }

        /// <summary>
        /// The format which the screenshots are saved
        /// </summary>
        public ScreenshotFormat Format { get; }

        /// <inheritdoc />
        public void OnNext(ScreenshotInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Observer.OnNext(value.FileName + Format.Extension);
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            Observer.OnError(error);
        }

        /// <inheritdoc />
        public void OnCompleted()
        {
            Observer.OnCompleted();
        }
    }
}
