using System;
using Tranquire.Reporting;

namespace Tranquire.Selenium.Extensions;

/// <summary>
/// Represent an observer that adapts the <see cref="ScreenshotInfo"/> notifications to a <see cref="ActionFileAttachment"/> notification that is passed to the new observer
/// </summary>
public sealed class ScreenshotInfoToActionAttachmentObserverAdapter : IObserver<ScreenshotInfo>
{
    /// <summary>
    /// Creates a new instance of<see cref="ScreenshotInfoToActionAttachmentObserverAdapter"/>
    /// </summary>
    /// <param name="attachmentObserver"></param>
    /// <param name="format">The format which the screenshots are saved</param>
    public ScreenshotInfoToActionAttachmentObserverAdapter(IObserver<ActionFileAttachment> attachmentObserver, ScreenshotFormat format)
    {
        AttachmentObserver = attachmentObserver ?? throw new ArgumentNullException(nameof(attachmentObserver));
        Format = format ?? throw new ArgumentNullException(nameof(format));
    }

    /// <summary>
    /// Gets the attachment observer to adapt the notifications
    /// </summary>
    public IObserver<ActionFileAttachment> AttachmentObserver { get; }

    /// <summary>
    /// Gets the format which the screenshots are saved
    /// </summary>
    public ScreenshotFormat Format { get; }

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

        AttachmentObserver.OnNext(new ActionFileAttachment(value.FileName + Format.Extension, string.Empty));
    }
}
