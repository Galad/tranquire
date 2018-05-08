using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Reporting;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// Represent an observer that adapts the <see cref="ScreenshotInfo"/> notifications to a <see cref="ActionFileAttachment"/> notification that is passed to the new observer
    /// </summary>
    public sealed class ScreenshotInfoToActionAttachmentObserverAdapter : IObserver<ScreenshotInfo>
    {
        /// <summary>
        /// Gets the attachment observer to adapt the notifications
        /// </summary>
        public IObserver<ActionFileAttachment> AttachmentObserver { get; }

        /// <summary>
        /// Creates a new instance of<see cref="ScreenshotInfoToActionAttachmentObserverAdapter"/>
        /// </summary>
        /// <param name="attachmentObserver"></param>
        public ScreenshotInfoToActionAttachmentObserverAdapter(IObserver<ActionFileAttachment> attachmentObserver)
        {
            AttachmentObserver = attachmentObserver ?? throw new ArgumentNullException(nameof(attachmentObserver));
        }

        /// <inheritdoc />
        public void OnCompleted()
        {
            AttachmentObserver.OnCompleted();
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            AttachmentObserver.OnError(error);
        }

        /// <inheritdoc />
        public void OnNext(ScreenshotInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            AttachmentObserver.OnNext(new ActionFileAttachment(value.FileName + ".jpg", string.Empty));
        }
    }
}
