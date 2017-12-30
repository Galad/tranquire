using System;
using Tranquire.Reporting;

namespace Tranquire.Selenium
{
    class NullScreenshotObserver : IObserver<ScreenshotNotification> {
        public void OnCompleted()
        {
            // do nothing
        }

        public void OnError(Exception error)
        {
            // do nothing
        }

        public void OnNext(ScreenshotNotification value)
        {
            // do nothing
        }
    }
}
