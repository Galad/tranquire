namespace Tranquire.Reporting;

internal interface INotificationHandler
{
    void Handle(XmlDocumentObserver observer, INamed named);
}