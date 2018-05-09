using System;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// Represent an observer the render a <see cref="ScreenshotInfo"/> to a string and send to to the inner observer
    /// </summary>
    public sealed class RenderedScreenshotInfoObserver : IObserver<ScreenshotInfo>
    {
        /// <summary>
        /// Gets the observer
        /// </summary>
        public IObserver<string> Observer { get; }

        /// <summary>
        /// Creates a new instance of <see cref="RenderedScreenshotInfoObserver"/>
        /// </summary>
        /// <param name="observer">The observer to render to</param>
        public RenderedScreenshotInfoObserver(IObserver<string> observer)
        {
            this.Observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }

        /// <inheritdoc />
        public void OnNext(ScreenshotInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Observer.OnNext(value.FileName + ".jpg");
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
